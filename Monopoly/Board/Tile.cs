namespace Monopoly
{
    public abstract class Tile : ITile
    {
        public string DisplayName { get; private set; }
        public int BoardPosition { get; private set; }

        public Player Visitor { get; private set; }

        public Tile(string displayName, int boardPosition)
        {
            DisplayName = displayName;
            BoardPosition = boardPosition;
        }

        public virtual void OnVisit(Player visitor, Board board)
        {
            Visitor = visitor;
        }
    }

    public class TileOptions
    {
        public string DisplayName { get; internal set; }

        public TileOptions(string displayName)
        {
            DisplayName = displayName;
        }
    }
}
