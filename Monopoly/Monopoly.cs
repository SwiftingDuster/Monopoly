using System;
using System.Collections.Generic;
using System.Linq;

namespace Monopoly
{
    public class Monopoly
    {
        public Queue<Player> Players { get; set; }

        private bool gameRunning;
        private Board board;

        public Monopoly(int humanPlayers, int aiPlayers)
        {
            Players = new Queue<Player>();

            for (int i = 0; i < humanPlayers; i++)
            {
                Players.Enqueue(new Player($"Player {i + 1}"));
            }

            for (int i = humanPlayers; i < humanPlayers + aiPlayers; i++)
            {
                Players.Enqueue(new Player($"Player {i + 1} [AI]", false));
            }
        }

        public Monopoly SetUp()
        {
            board = new Board(this);
            return this;
        }

        public void Start()
        {
            gameRunning = true;
            int round = 1;
            while (gameRunning)
            {
                Console.WriteLine($"> Starting round {round++}.");
                for (int i = 0; i < Players.Count; i++)
                {
                    Player player = Players.Dequeue();
                    player.TakeTurn(board);
                    if (!player.IsBankrupt) Players.Enqueue(player);
                }
                Console.WriteLine("====================");

                if (Players.Count == 1)
                {
                    break;
                }
            }

            Player winner = Players.First();
            int totalAssetValue = winner.OwnedTiles.Where(tile => !tile.IsMortgaged).Sum(tile => tile.TileOptions.Cost);

            Console.WriteLine($"{winner.DisplayName} won the game with ${winner.Money}, {winner.OwnedTiles.Count} owned properties and a total asset value of ${totalAssetValue}.");
            Console.ReadKey();
        }
    }
}
