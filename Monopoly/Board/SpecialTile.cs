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
                    Console.WriteLine($"{visitor.DisplayName} receives {Constants.GOAllowance} from the bank.");
                    break;
                case SpecialTileType.Chance:
                    break;
                case SpecialTileType.CommunityChest:
                    break;
                case SpecialTileType.VisitingJail:
                    break;
                case SpecialTileType.GoToJail:
                    visitor.BoardLocation = board.GetSpecialTileIndex(SpecialTileType.GoToJail)[0];
                    Console.WriteLine($"{visitor.DisplayName} was sent to jail.");
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
