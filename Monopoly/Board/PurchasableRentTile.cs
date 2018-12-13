using System;
using System.Collections.Generic;

namespace Monopoly
{
    public abstract class PurchasableRentTile : Tile, IRentTile
    {
        public abstract int Rent { get; }
        public abstract PurchasableTileOptions TileOptions { get; }

        public Player Owner { get; protected set; }

        public bool IsMortgaged { get; protected set; }

        public PurchasableRentTile(string displayName, int boardPosition) : base(displayName, boardPosition) { }

        public void Purchase(Player player)
        {
            if (Owner)
            {
                throw new Exception("Attempt to purchase property with existing owner!");
            }

            if (CanPurchase(player))
            {
                player.Money -= TileOptions.Cost;
                player.OwnedTiles.Add(this);
                Owner = player;
            }
        }

        public bool CanPurchase(Player player)
        {
            if (Owner) return false;

            return player.Money > TileOptions.Cost;
        }

        public virtual IEnumerable<int> Mortgage()
        {
            if (!Owner) throw new Exception("Attempt to get mortgaged property of tile without owner!");

            if (IsMortgaged) yield break;

            IsMortgaged = true;
            Owner.Money += TileOptions.MortgageValue;
            Owner.MortgagedTiles.Add(this);
            Console.WriteLine($"{Owner.DisplayName} has mortgaged {DisplayName} for ${TileOptions.MortgageValue}.");

            yield return TileOptions.MortgageValue;
        }

        public void UnMortgage()
        {
            if (!Owner) throw new Exception("Attempt to get mortgaged property of tile without owner!");

            if (!IsMortgaged) return;

            int unmortgageCost = TileOptions.MortgageValue * 11 / 10;
            if (Owner.Money < unmortgageCost) return;

            IsMortgaged = false;
            Owner.Money -= unmortgageCost;
            Owner.MortgagedTiles.Remove(this);
            Console.WriteLine($"{Owner.DisplayName} paid ${unmortgageCost} including 10% interest to unmortgage {DisplayName}.");
        }

        public void Disown()
        {
            Owner = null;
            IsMortgaged = false;
        }
    }

    public class PurchasableTileOptions : TileOptions
    {
        public int Cost { get; private set; }
        public int MortgageValue { get; private set; }
        public int UnMortgageValue { get; private set; }

        public PurchasableTileOptions(string displayName, int cost, int mortageValue) : base(displayName)
        {
            Cost = cost;
            MortgageValue = mortageValue;
            UnMortgageValue = mortageValue * 11 / 10;
        }
    }
}
