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

        public int BoardLocation { get; set; }
        public int Money { get; set; }
        public List<PurchasableRentTile> OwnedTiles { get; set; }
        
        public Player(string name, bool isHuman = true)
        {
            DisplayName = name;
            IsHuman = isHuman;

            Money = 1000;
            OwnedTiles = new List<PurchasableRentTile>();
        }

        public virtual void TakeTurn(Board board)
        {
            DiceRoll = random.Next(1, 7);
            BoardLocation += DiceRoll;
            if (BoardLocation >= board.TileSize)
            {
                BoardLocation %= board.TileSize;

                // Passes GO but did not stop on GO.
                if (BoardLocation != 0)
                {
                    board.GetTile(0).OnVisit(this, board);
                }
            }
            Tile tile = board.GetTile(BoardLocation);

            Console.WriteLine($"{DisplayName} [${Money} | {OwnedTiles.Count} owned properties].");
            Console.WriteLine($"{DisplayName} rolled a {DiceRoll}.");
            Console.WriteLine($"{DisplayName} is visiting {tile.DisplayName}.");
            tile.OnVisit(this, board);

            Console.WriteLine("--------------------");
        }

        public void Pay(Player recipient, ref int amount)
        {
            if (Money < amount)
            {
                Console.WriteLine($"{DisplayName} only has ${Money} of {amount} to pay.");

                var unmortgagedTiles = OwnedTiles.Where(tile => !tile.IsMortgaged).OrderBy(tile => tile.TileOptions.Cost);
                if (unmortgagedTiles.Count() > 0)
                {
                    do
                    {
                        PurchasableRentTile tile = unmortgagedTiles.First();
                        tile.IsMortgaged = true;
                        Console.WriteLine($"{DisplayName} has mortgaged {tile.DisplayName} for ${tile.TileOptions.MortgageValue}");
                    }
                    while (Money < amount && unmortgagedTiles.Count() > 0);
                }
                
                if (Money < amount) // After mortgaging (or nothing to mortgage) and still unable to pay, pay with whatever the player is left and disown all properties.
                {
                    amount = Money;
                    OwnedTiles.ForEach(tile => tile.Disown());
                    IsBankrupt = true;
                    Console.WriteLine($"[BANKRUPTED] {DisplayName} is now bankrupt.");
                }
            }
            recipient.Money += amount;
            Money -= amount;
        }

        public void ReceiveMoneyFromBank(int amount)
        {
            Money += amount;
        }

        public static implicit operator bool(Player player)
        {
            return player != null;
        }
    }
}
