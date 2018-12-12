using Monopoly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Monopoly
{
    public static class SpecialCardDeck
    {
        private static readonly List<ChanceCard> ChanceCards;
        private static readonly List<CommunityChest> CommunityChests;

        private static Random Random;
        
        static SpecialCardDeck()
        {
            ChanceCards = new List<ChanceCard>();
            FieldInfo[] chanceCards = typeof(ChanceCard).GetFields(BindingFlags.Public | BindingFlags.Static);
            chanceCards.ToList().ForEach(card => ChanceCards.Add(card.GetValue(null) as ChanceCard));

            CommunityChests = new List<CommunityChest>();
            FieldInfo[] communityChests = typeof(CommunityChest).GetFields(BindingFlags.Public | BindingFlags.Static);
            communityChests.ToList().ForEach(card => CommunityChests.Add(card.GetValue(null) as CommunityChest));

            Random = new Random();
        }
        
        public static ChanceCard GetChanceCard()
        {
            int rand = Random.Next(ChanceCards.Count);
            return ChanceCards[rand];
        }

        public static CommunityChest GetCommunityChest()
        {
            int rand = Random.Next(CommunityChests.Count);
            return CommunityChests[rand];
        }
    }

    public class ChanceCard : Card
    {
        public static ChanceCard AdvanceToGo = new ChanceCard("Advance to GO. Collect $200.", (player, board, players) =>
        {
            player.BoardLocation = board.GetSpecialTile(SpecialTileType.GO).BoardPosition;
        });
        public static ChanceCard AdvanceToIllinoisAvenue = new ChanceCard("Advance to Illinois Avenue.", (player, board, players) =>
        {
            player.BoardLocation = board.Find(tile => tile.DisplayName.Contains("Illinois Avenue")).BoardPosition;
        });
        public static ChanceCard AdvanceToStCharlesPlace = new ChanceCard("Advance to St. Clarles Place.", (player, board, players) =>
        {
            player.BoardLocation = board.Find(tile => tile.DisplayName.Contains("St. Charles Place")).BoardPosition;
        });
        public static ChanceCard AdvanceToNearestUtility = new ChanceCard("Advance to nearest Utility.", (player, board, players) =>
        {
            player.BoardLocation = board.FindNearest(NearestType.Utility, player.BoardLocation).BoardPosition;
        });
        public static ChanceCard AdvanceToNearestRailroad = new ChanceCard("Advance to nearest Railroad.", (player, board, players) =>
        {
            player.BoardLocation = board.FindNearest(NearestType.Railroad, player.BoardLocation).BoardPosition;
        });
        public static ChanceCard BankPaysDividend = new ChanceCard("Bank pays you diviend of $50.", (player, board, players) =>
        {
            player.Money += 50;
        });
        public static ChanceCard GetOutOfJailFree = new ChanceCard("Get out of Jail Free.", (player, board, players) =>
        {
            player.OutOfJailCards.Add(GetOutOfJailFree);
        });
        public static ChanceCard GoBack3Space = new ChanceCard("Go Back Three Spaces.", (player, board, players) =>
        {
            if (player.BoardLocation < 3) player.BoardLocation += Constants.GameTilesTotal;
            player.BoardLocation -= 3;
        });
        public static ChanceCard GoToJailDirectly = new ChanceCard("Go to Jail.", (player, board, players) =>
        {
            player.Jail(board);
        });
        public static ChanceCard GeneralRepairProperty = new ChanceCard("Make general repairs on all your property: For each house pay $25, for each hotel pay $100.", (player, board, players) =>
        {
            var properties = player.OwnedTiles.OfType<PropertyTile>();
            int houseTotalCost = properties.Where(p => p.HouseCount < Constants.MaxHouse).Sum(p => p.HouseCount) * 25;
            int hotelTotalCost = properties.Where(p => p.HouseCount == Constants.MaxHouse).Count() * 100;
            player.PayToBank(houseTotalCost + hotelTotalCost);
        });
        public static ChanceCard PayPoorTax = new ChanceCard("Pay poor tax of $15.", (player, board, players) =>
        {
            player.Money -= 15;
        });
        public static ChanceCard AdvanceToReadingRailroad = new ChanceCard("Take a trip to Reading Railroad.", (player, board, players) =>
        {
            player.BoardLocation = board.Find(tile => tile.DisplayName.Contains("Reading Railroad")).BoardPosition;
        });
        public static ChanceCard AdvanceToBroadwalk = new ChanceCard("Take a walk on the Broadwalk. Advance token to Broadwalk.", (player, board, players) =>
        {
            player.BoardLocation = board.Find(tile => tile.DisplayName.Contains("Broadwalk")).BoardPosition;
        });
        public static ChanceCard ElectedChairmanOfTheBoard = new ChanceCard("You have been elected Chairman of the Board. Pay each player $50.", (player, board, players) =>
        {
            foreach (Player otherPlayer in players)
            {
                if (player == otherPlayer) continue;
                int amount = 50;
                player.Pay(otherPlayer, ref amount);
            }
        });
        public static ChanceCard BuildingAndLoanMatures = new ChanceCard("Your building loan matures. Collect $150.", (player, board, players) =>
        {
            player.Money += 150;
        });
        public static ChanceCard WonCrosswordCompetition = new ChanceCard("You have won a crossword competition. Collect $100.", (player, board, players) =>
        {
            player.Money += 100;
        });

        public ChanceCard(string displayName, Action<Player, Board, Queue<Player>> action) : base(displayName, action) { }
    }

    public class CommunityChest : Card
    {
        public static CommunityChest AdvanceToGo = new CommunityChest("Advance to GO. Collect $200", (player, board, players) =>
        {
            player.BoardLocation = board.GetSpecialTile(SpecialTileType.GO).BoardPosition;
        });
        public static CommunityChest BankErrorInYourFavour = new CommunityChest("Bank error in your favour. Collect $200.", (player, board, players) =>
        {
            player.Money += 200;
        });
        public static CommunityChest DoctorFees = new CommunityChest("Doctor's Fees. Pay $50.", (player, board, players) =>
        {
            player.Money -= 50;
        });
        public static CommunityChest SaleOfStock = new CommunityChest("From sale of stock you get $50.", (player, board, players) =>
        {
            player.Money += 50;
        });
        public static CommunityChest GetOutOfJailFree = new CommunityChest("Get out of Jail Free.", (player, board, players) =>
        {
            player.OutOfJailCards.Add(GetOutOfJailFree);
        });
        public static CommunityChest GoToJailDirectly = new CommunityChest("Go to Jail.", (player, board, players) =>
        {
            player.Jail(board);
        });
        public static CommunityChest GrandOperaNight = new CommunityChest("Grand Opera Night. Collect $50 from every player for opening night seats.", (player, board, players) =>
        {
            foreach (Player otherPlayer in players)
            {
                if (player == otherPlayer) continue;
                int amount = 50;
                otherPlayer.Pay(player, ref amount);
            }
        });
        public static CommunityChest HolidayFundMatures = new CommunityChest("Holiday Fund matures. Collect $100.", (player, board, players) =>
        {
            player.Money += 100;
        });
        public static CommunityChest IncomeTaxRefund = new CommunityChest("Income tax refund. Collect $20.", (player, board, players) =>
        {
            player.Money += 20;
        });
        public static CommunityChest ItsYourBirthday = new CommunityChest("It's your birthday. Collect $10 from every player.", (player, board, players) =>
        {
            foreach (Player otherPlayer in players)
            {
                if (player == otherPlayer) continue;
                int amount = 10;
                otherPlayer.Pay(player, ref amount);
            }
        });
        public static CommunityChest LifeInsuranceMatures = new CommunityChest("Life insurance matures. Collect $100.", (player, board, players) =>
        {
            player.Money += 100;
        });
        public static CommunityChest HospitalFees = new CommunityChest("Hospital Fees. Pay $50.", (player, board, players) =>
        {
            player.Money -= 50;
        });
        public static CommunityChest SchoolFees = new CommunityChest("School fees. Pay $50.", (player, board, players) =>
        {
            player.Money -= 50;
        });
        public static CommunityChest ReceiveConsultancyFee = new CommunityChest("Receieve $25 consultancy fee.", (player, board, players) =>
        {
            player.Money += 25;
        });
        public static CommunityChest AssessedForStreetRepairs = new CommunityChest("You are assessed for street repairs: Pay $40 per house and $115 per hotel you own.", (player, board, players) =>
        {
            var properties = player.OwnedTiles.OfType<PropertyTile>();
            int houseTotalCost = properties.Where(p => p.HouseCount < Constants.MaxHouse).Sum(p => p.HouseCount) * 40;
            int hotelTotalCost = properties.Where(p => p.HouseCount == Constants.MaxHouse).Count() * 115;
            player.PayToBank(houseTotalCost + hotelTotalCost);
        });
        public static CommunityChest WonSecondInBeautyContest = new CommunityChest("You have won second prize in a beauty contest. Collect $10.", (player, board, players) =>
        {

        });
        public static CommunityChest InheritMoney = new CommunityChest("You inherit $100.", (player, board, players) =>
        {
            player.Money += 10;
        });

        public CommunityChest(string displayName, Action<Player, Board, Queue<Player>> action) : base(displayName, action) { }
    }

    public class Card
    {
        public string DisplayName { get; set; }
        public Action<Player, Board, Queue<Player>> Use { get; set; }

        public Card(string displayName, Action<Player, Board, Queue<Player>> action)
        {
            DisplayName = displayName;
            Use = action;
        }
    }
}