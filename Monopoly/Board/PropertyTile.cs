using System;
using System.Collections.Generic;

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
                    if (IsMortgaged)
                    {
                        Console.WriteLine($"{visitor.DisplayName} was exempted rent because {DisplayName} is currently mortgaged.");
                        return;
                    }

                    int rent = Rent;
                    visitor.Pay(Owner, ref rent);
                    Console.WriteLine($"{visitor.DisplayName} paid ${rent} to {Owner.DisplayName} for visiting {tileOptions.DisplayName}. [Houses x{HouseCount}].");
                }
                else // Owner visiting
                {
                    if (IsMortgaged)
                    {
                        Console.WriteLine($"{DisplayName} cannot be upgraded because it is currently mortgaged.");
                        return;
                    }

                    if (CanUpgrade(Owner))
                    {
                        if (Owner.IsHuman)
                        {
                            Console.WriteLine($"Do you want to upgrade {DisplayName} to {HouseCount + 1} houses for ${tileOptions.HouseCost}? (Y/n)");
                        }

                        if (!visitor.IsHuman || Console.ReadKey(true).Key == ConsoleKey.Y)
                        {
                            Upgrade();
                            Console.WriteLine($"{Owner.DisplayName} upgraded {DisplayName} to {HouseCount} houses for ${tileOptions.HouseCost}.");
                        }
                    }
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

        public int SellUpgrade(int priceCut = 2)
        {
            if (Owner)
            {
                if (HouseCount > 0)
                {
                    int returnPrice = tileOptions.HouseCost / priceCut;
                    Owner.Money += returnPrice;
                    Console.WriteLine($"{Owner.DisplayName} sold house {HouseCount--} of {DisplayName} at 1/{priceCut} price for ${returnPrice}.");
                    return returnPrice;
                }
            }
            return 0;
        }

        public override IEnumerable<int> Mortgage()
        {
            if (!Owner) throw new Exception("Attempt to get mortgaged property of tile without owner!");

            if (IsMortgaged) yield break;

            while (HouseCount > 0)
            {
                yield return SellUpgrade();
            }

            IsMortgaged = true;
            Owner.Money += TileOptions.MortgageValue;
            Owner.MortgagedTiles.Remove(this);
            Console.WriteLine($"{Owner.DisplayName} has mortgaged {DisplayName} for ${TileOptions.MortgageValue}.");

            yield return TileOptions.MortgageValue;
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
