using System;

namespace Monopoly
{
    public abstract class PurchasableRentTile : Tile, IRentTile
    {
        public abstract int Rent { get; }
        public abstract PurchasableTileOptions TileOptions { get; }

        public Player Owner { get; protected set; }

        public bool IsMortgaged
        {
            get
            {
                return isMortgaged;
            }
            set
            {
                if (!Owner) throw new Exception("Attempt to get mortgaged property of tile without owner!");

                if (value)
                {
                    Owner.Money += TileOptions.MortgageValue;
                }
                else
                {
                    if (Owner.Money < TileOptions.MortgageValue) return;

                    Owner.Money -= TileOptions.MortgageValue;
                }

                isMortgaged = value;
            }
        }
        private bool isMortgaged;

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

        public void Disown()
        {
            Owner = null;
        }
    }

    public class PurchasableTileOptions : TileOptions
    {
        public int Cost { get; set; }
        public int MortgageValue { get; set; }

        public PurchasableTileOptions(string displayName, int cost, int mortageValue) : base(displayName)
        {
            Cost = cost;
            MortgageValue = mortageValue;
        }
    }
}
