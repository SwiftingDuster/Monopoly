using System;
using System.Linq;

namespace Monopoly
{
    public class RailroadTile : PurchasableRentTile
    {
        public override int Rent
        {
            get
            {
                if (!Owner) throw new Exception("Attempt to get rent of unowned tile.");

                int count = Owner.OwnedTiles.Where(tile => tile is RailroadTile).Count();
                switch (count)
                {
                    case 1:
                        return Constants.RailroadOwn1Rent;
                    case 2:
                        return Constants.RailroadOwn2Rent;
                    case 3:
                        return Constants.RailroadOwn3Rent;
                    case 4:
                        return Constants.RailroadOwn4Rent;
                    default:
                        throw new Exception($"Number of railroad tile is invalid. Value: {count} Expected: 1,2,3,4");
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
        private readonly RailroadTileOptions tileOptions;

        public RailroadTile(int boardPosition, RailroadTileOptions options) : base(options.DisplayName, boardPosition)
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
                    Console.WriteLine($"{visitor.DisplayName} paid ${rent} to {Owner.DisplayName} for visiting {tileOptions.DisplayName} [Railroads x{Owner.OwnedTiles.Where(tile => tile is UtilityTile).Count()}]");
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

    public class RailroadTileOptions : PurchasableTileOptions
    {
        public RailroadTileOptions(string displayName, int cost, int mortageValue) : base(displayName, cost, mortageValue) { }
    }
}
