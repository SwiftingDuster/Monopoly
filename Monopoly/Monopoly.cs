using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Monopoly
{
    public class Monopoly
    {
        private bool gameRunning;
        private Board board;
        private Queue<Player> players;

        public Monopoly(int humanPlayers, int aiPlayers)
        {
            players = new Queue<Player>();

            for (int i = 0; i < humanPlayers; i++)
            {
                players.Enqueue(new Player($"Player {i + 1}"));
            }

            for (int i = humanPlayers; i < humanPlayers + aiPlayers; i++)
            {
                players.Enqueue(new Player($"Player {i + 1} [AI]", false));
            }
        }

        public Monopoly SetUp()
        {
            board = new Board();
            return this;
        }

        public void Start()
        {
            gameRunning = true;
            int round = 1;
            while (gameRunning)
            {
                Console.WriteLine($"> Starting round {round++}.");
                for (int i = 0; i < players.Count; i++)
                {
                    Player player = players.Dequeue();
                    player.TakeTurn(board);
                    if (!player.IsBankrupt) players.Enqueue(player);
                }
                Console.WriteLine("====================");

                if (players.Count == 1)
                {
                    break;
                }
            }

            Player winner = players.First();
            int totalAssetValue = winner.OwnedTiles.Where(tile => !tile.IsMortgaged).Sum(tile => tile.TileOptions.Cost);

            Console.WriteLine($"{winner.DisplayName} won the game with ${winner.Money}, {winner.OwnedTiles.Count} owned properties and a total asset value of ${totalAssetValue}.");
            Console.ReadKey();
        }
    }
}
