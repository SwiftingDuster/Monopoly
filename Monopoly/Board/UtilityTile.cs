﻿using System;
using System.Linq;

namespace Monopoly
{
    public class UtilityTile : PurchasableRentTile
    {
        public override int Rent
        {
            get
            {
                if (!Owner) throw new Exception("Attempt to get rent of unowned tile.");

                int count = Owner.OwnedTiles.Where(tile => tile is UtilityTile).Count();
                switch (count)
                {
                    case 1:
                        return Constants.UtilityOwn1DiceX * Visitor.DiceRoll;
                    case 2:
                        return Constants.UtilityOwn2DiceX * Visitor.DiceRoll;
                    default:
                        throw new Exception($"Number of utility tile is invalid. Value: {count} Expected: 1,2");
                }
            }
        }

        public override PurchasableTileOptions TileOptions
        {
            get
            {
                return tileOptions;
            }
        }
        private readonly UtilityTileOptions tileOptions;

        public UtilityTile(int boardPosition, UtilityTileOptions options) : base(options.DisplayName, boardPosition)
        {
            tileOptions = options;
        }

        public override void OnVisit(Player visitor, Board board)
        {
            base.OnVisit(visitor, board);

            if (Owner) // Owned property
            {
                if (visitor != Owner) // Other player visiting
                {
                    int rent = Rent;
                    visitor.Pay(Owner, ref rent);
                    Console.WriteLine($"{visitor.DisplayName} paid ${rent} to {Owner.DisplayName} for visiting {tileOptions.DisplayName} [Utility x{Owner.OwnedTiles.Where(tile => tile is UtilityTile).Count()}]");
                }
            }
            else // Unowned property
            {
                if (CanPurchase(visitor))
                {
                    if (visitor.IsHuman)
                    {
                        Console.WriteLine($"Do you want to purchase {DisplayName} for ${tileOptions.Cost}? (Y/n)");
                    }

                    if (!visitor.IsHuman || Console.ReadKey(true).Key == ConsoleKey.Y)
                    {
                        Purchase(visitor);
                        Console.WriteLine($"{visitor.DisplayName} purchased {DisplayName} for ${tileOptions.Cost}.");
                    }
                }
            }
        }
    }

    public class UtilityTileOptions : PurchasableTileOptions
    {
        public UtilityTileOptions(string displayName, int cost, int mortageValue) : base(displayName, cost, mortageValue) { }
    }
}
