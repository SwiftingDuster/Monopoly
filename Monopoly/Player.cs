using System;
using System.Collections.Generic;
using System.Linq;

namespace Monopoly
{
    public class Player
    {
        private static Random random = new Random();

        public string DisplayName { get; private set; }
        public bool IsHuman { get; private set; }
        public int DiceRoll { get; private set; }
        public bool IsBankrupt { get; private set; }

        public int BoardLocation { get; private set; }
        public int Money { get; set; }
        public List<PurchasableRentTile> OwnedTiles { get; set; }
        public List<PurchasableRentTile> MortgagedTiles { get; set; }
        public List<Card> OutOfJailCards { get; set; }
        public int JailTurnRemaining { get; set; }

        private int boardLocation;

        public Player(string name, bool isHuman = true)
        {
            DisplayName = name;
            IsHuman = isHuman;

            Money = 1000;
            OwnedTiles = new List<PurchasableRentTile>();
            MortgagedTiles = new List<PurchasableRentTile>();
            OutOfJailCards = new List<Card>();
        }

        public virtual void TakeTurn(Board board)
        {
            Console.WriteLine($"{DisplayName} [${Money} | {OwnedTiles.Count} properties | {MortgagedTiles.Count} mortgaged].");

            if (boardLocation == board.GetSpecialTile(SpecialTileType.Jail).BoardPosition && JailTurnRemaining > 0) // In jail
            {
                Console.WriteLine($"{DisplayName} is in jail. {JailTurnRemaining} turn{(JailTurnRemaining > 1 ? "s" : "")} remaining.");
                if (OutOfJailCards.Count > 0)
                {
                    if (IsHuman)
                    {
                        Console.WriteLine($"Do you want to use 1 Get out of Jail card? (You have {OutOfJailCards.Count}) (Y/n)");
                    }
                    
                    if (!IsHuman || Console.ReadKey(true).Key == ConsoleKey.Y)
                    {
                        JailTurnRemaining = 0;
                        Console.WriteLine($"{DisplayName} used 1 Get out of Jail card.");
                    }
                }
            }
            else
            {
                DiceRoll = random.Next(1, 7);
                Tile tile = AdvanceBoardPostiion(board, DiceRoll, false);
                
                Console.WriteLine($"{DisplayName} rolled a {DiceRoll}.");
                Console.WriteLine($"{DisplayName} is visiting {tile.DisplayName}. {((tile is PropertyTile && ((PropertyTile)tile).Owner != null) ? $"[{((PropertyTile)tile).Owner.DisplayName}]" : String.Empty)}");
                tile.OnVisit(this, board);
            }
            
            var canUnmortgage = MortgagedTiles.Where(tile => Money > tile.TileOptions.UnMortgageValue).ToList();
            if (canUnmortgage.Count > 0)
            {
                if (IsHuman)
                {
                    ConsoleKey key;
                    do
                    {
                        Console.WriteLine($"Do you want to buy back any mortgaged property? (Y/n)");
                        key = Console.ReadKey(true).Key;

                        if (key == ConsoleKey.Y)
                        {
                            for (int i = 0; i < canUnmortgage.Count(); i++)
                            {
                                PurchasableRentTile tile = canUnmortgage[i];
                                Console.WriteLine($"Choose the property to unmortgage by entering the corressponding number {i + 1}-{canUnmortgage.Count() - 1}:");
                                Console.WriteLine($"{i + 1}. {tile.DisplayName} for ${tile.TileOptions.UnMortgageValue}.");
                            }

                            char c;
                            bool isDigit, isValid;
                            int choice = 0;
                            do
                            {
                                c = Console.ReadKey(true).KeyChar;
                                isDigit = isValid = char.IsDigit(c);
                                if (c == 'q' || c == 'Q') // Quit
                                {
                                    key = ConsoleKey.N;
                                    break;
                                }
                                else if (!isDigit)
                                {
                                    Console.WriteLine("The value you have entered is not a number. To exit, enter 'Q'.");
                                }
                                else
                                {
                                    choice = int.Parse(c.ToString());
                                    if (choice < 0 || choice >= canUnmortgage.Count())
                                    {
                                        Console.WriteLine("The number you have entered is not a valid choice.");
                                        isValid = false;
                                    }
                                }
                            }
                            while (!(isDigit && isValid));

                            PurchasableRentTile tileToBuyBack = canUnmortgage[choice];
                            tileToBuyBack.UnMortgage();
                        }
                    }
                    while (key == ConsoleKey.Y);
                }
                else
                {
                    canUnmortgage = canUnmortgage.OrderBy(tile => tile.TileOptions.UnMortgageValue).ToList();
                    while (MortgagedTiles.Count > 0 && Money > canUnmortgage.Min(tile => tile.TileOptions.UnMortgageValue * 3 / 2))
                    {
                        PurchasableRentTile tileToBuyBack = canUnmortgage.SkipWhile(tile => !tile.IsMortgaged).First();
                        tileToBuyBack.UnMortgage();
                    }
                }
            }

            if (JailTurnRemaining > 0) JailTurnRemaining--;

            Console.WriteLine($"{DisplayName} [${Money} | {OwnedTiles.Count} properties | {OwnedTiles.Where(tile => tile.IsMortgaged).Count()} mortgaged].");
        }

        public void Pay(Player recipient, ref int amount)
        {
            PayToBank(ref amount);
            recipient.Money += amount;
        }

        public void PayToBank(ref int amount)
        {
            if (Money < amount)
            {
                Console.WriteLine($"{DisplayName} only has ${Money} of ${amount} to pay.");

                var unmortgagedTiles = OwnedTiles.Where(tile => !tile.IsMortgaged).OrderBy(tile => tile.TileOptions.Cost);
                if (unmortgagedTiles.Count() > 0)
                {
                    foreach (var tile in unmortgagedTiles)
                    {
                        foreach (int soldPrice in tile.Mortgage())
                        {
                            if (Money > amount) break; // Sold enough
                        }
                        if (Money > amount) break; // Sold enough
                    }
                }

                if (Money < amount) // After mortgaging (or nothing to mortgage) and still unable to pay, pay with whatever the player is left and disown all properties.
                {
                    amount = Money;
                    OwnedTiles.ForEach(tile => tile.Disown());
                    OwnedTiles.Clear();
                    IsBankrupt = true;
                }
            }
            Money -= amount;
        }

        public void ReceiveMoneyFromBank(int amount)
        {
            Money += amount;
        }

        public Tile AdvanceBoardPostiion(Board board, int positionOffset, bool visit = false)
        {
            boardLocation += positionOffset;
            if (boardLocation >= board.TileSize)
            {
                boardLocation %= board.TileSize;

                // Passes GO but did not stop on GO.
                if (boardLocation != 0)
                {
                    board.GetSpecialTile(SpecialTileType.GO).OnVisit(this, board);
                }
            }
            else if (boardLocation < 0)
            {
                boardLocation += Constants.GameTilesTotal;
            }

            Tile tile = board.GetTile(boardLocation);
            if (visit)
            {
                tile.OnVisit(this, board);
            }
            return tile;
        }

        public Tile MoveToBoardPostiion(Board board, int position, bool visit = true)
        {
            if (position < 0 || position >= Constants.GameTilesTotal) throw new Exception($"Position cannot be negative or higher than {Constants.GameTilesTotal}.");

            boardLocation = position;

            Tile tile = board.GetTile(boardLocation);
            if (visit)
            {
                tile.OnVisit(this, board);
            }
            return tile;
        }

        public void Jail(Board board)
        {
            MoveToBoardPostiion(board, board.GetSpecialTile(SpecialTileType.Jail).BoardPosition, false); // Don't call OnVisit again otherwise its an infinite loop.
            JailTurnRemaining = 3;
            Console.WriteLine($"{DisplayName} was sent to jail.");
        }

        public static implicit operator bool(Player player)
        {
            return player != null;
        }
    }
}
