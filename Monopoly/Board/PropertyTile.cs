using System;

namespace Monopoly
{
    public class PropertyTile : PurchasableRentTile, IUpgradable
    {
        public override int Rent
        {
            get
            {
                switch (HouseCount)
                {
                    case 0:
                        return tileOptions.RentBase;
                    case 1:
                        return tileOptions.RentHouse1;
                    case 2:
                        return tileOptions.RentHouse2;
                    case 3:
                        return tileOptions.RentHouse3;
                    case 4:
                        return tileOptions.RentHouse4;
                    case 5:
                        return tileOptions.RentHotel;
                    default:
                        throw new Exception("House count value is invalid.");
                }
            }
        }

        public int HouseCount { get; set; }

        public override PurchasableTileOptions TileOptions
        {
            get
            {
                return tileOptions;
            }
        }
        private readonly PropertyTileOptions tileOptions;

        public PropertyTile(int boardPosition, PropertyTileOptions options) : base(options.DisplayName, boardPosition)
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
                    Console.WriteLine($"{visitor.DisplayName} paid {rent} to {Owner.DisplayName} for visiting {tileOptions.DisplayName} [Houses: {HouseCount}]");
                }
                else // Owner visiting
                {
                    if (CanUpgrade(Owner))
                    {
                        if (Owner.IsHuman)
                        {
                            Console.WriteLine($"Do you want to upgrade {DisplayName} for {tileOptions.HouseCost}? (Y/n)");
                            if (Console.ReadKey().Key != ConsoleKey.Y) return;
                        }

                        Upgrade();
                        Console.WriteLine($"{Owner.DisplayName} upgraded {DisplayName} to {HouseCount} houses for {tileOptions.HouseCost}.");
                    }
                }
            }
            else // Unowned property
            {
                if (CanPurchase(visitor))
                {
                    if (visitor.IsHuman)
                    {
                        Console.WriteLine($"Do you want to purchase {DisplayName} for {tileOptions.Cost}? (Y/n)");
                        if (Console.ReadKey().Key != ConsoleKey.Y) return;
                    }

                    Purchase(visitor);
                    Console.WriteLine($"{visitor.DisplayName} purchased {DisplayName} for {tileOptions.Cost}.");
                }
            }
        }

        public void Upgrade()
        {
            if (!Owner)
            {
                throw new Exception("Attempt to upgrade property without owner!");
            }

            if (CanUpgrade(Owner))
            {
                HouseCount++;
                Owner.Money -= tileOptions.HouseCost;
            }
        }

        public bool CanUpgrade(Player player)
        {
            if (!Owner) return false;

            if (HouseCount < 4)
            {
                return player.Money > tileOptions.HouseCost;
            }
            else
            {
                return false;
            }
        }
    }

    public class PropertyTileOptions : PurchasableTileOptions
    {
        public int HouseCost { get; set; }
        public int RentBase { get; set; }
        public int RentHouse1 { get; set; }
        public int RentHouse2 { get; set; }
        public int RentHouse3 { get; set; }
        public int RentHouse4 { get; set; }
        public int RentHotel { get; set; }

        public PropertyTileOptions(string displayName, int cost, int houseCost, int mortageValue, int rentBase,
            int rentHouse1, int rentHouse2, int rentHouse3, int rentHouse4, int rentHotel) : base(displayName, cost, mortageValue)
        {
            HouseCost = houseCost;
            RentBase = rentBase;
            RentHouse1 = rentHouse1;
            RentHouse2 = rentHouse2;
            RentHouse3 = rentHouse3;
            RentHouse4 = rentHouse4;
            RentHotel = rentHotel;
        }
    }
}
