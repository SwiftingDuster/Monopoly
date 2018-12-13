using System;

namespace Monopoly
{
    public class SpecialTile : Tile
    {
        private readonly TileOptions tileOptions;
        private readonly SpecialTileType specialTileType;

        public SpecialTile(int boardPosition, TileOptions options, SpecialTileType type) : base(options.DisplayName, boardPosition)
        {
            tileOptions = options;
            specialTileType = type;
        }

        public override void OnVisit(Player visitor, Board board)
        {
            base.OnVisit(visitor, board);

            switch (specialTileType)
            {
                case SpecialTileType.GO:
                    visitor.ReceiveMoneyFromBank(Constants.GOAllowance);
                    Console.WriteLine($"{visitor.DisplayName} receives {Constants.GOAllowance} from the bank for passing GO.");
                    break;
                case SpecialTileType.Chance:
                    ChanceCard chanceCard = SpecialCardDeck.GetChanceCard();
                    Console.WriteLine($"{visitor.DisplayName} drew chance card: {chanceCard.DisplayName}");
                    chanceCard.Use(visitor, board, board.Monopoly.Players);
                    break;
                case SpecialTileType.CommunityChest:
                    CommunityChest community = SpecialCardDeck.GetCommunityChest();
                    Console.WriteLine($"{visitor.DisplayName} drew community chest: {community.DisplayName}");
                    community.Use(visitor, board, board.Monopoly.Players);
                    break;
                case SpecialTileType.VisitingJail:
                    break;
                case SpecialTileType.Jail:
                    visitor.Jail(board);
                    break;
                case SpecialTileType.FreeParking:
                    break;
                case SpecialTileType.IncomeTax:
                    break;
                case SpecialTileType.LuxuryTax:
                    break;
                default:
                    throw new Exception($"Unhandled special tile type: {specialTileType}");
            }
        }
    }
}
