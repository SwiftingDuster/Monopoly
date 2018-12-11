using System;

namespace Monopoly
{
    public class Board
    {
        public int TileSize { get; private set; }

        private readonly Tile[] tiles;

        public Board(int tileSize = 40)
        {
            TileSize = tileSize;
            tiles = new Tile[tileSize];

            tiles[0] = new SpecialTile(0, new TileOptions("GO"), SpecialTileType.GO);
            tiles[1] = new PropertyTile(1, new PropertyTileOptions("Mediterranean Avenue", 60, 50, 30, 2, 10, 30, 90, 160, 250));
            tiles[2] = new SpecialTile(2, new TileOptions("Community Chest"), SpecialTileType.CommunityChest);
            tiles[3] = new PropertyTile(3, new PropertyTileOptions("Baltic Avenue", 60, 50, 30, 4, 10, 60, 180, 320, 450));
            tiles[4] = new SpecialTile(4, new TileOptions("Income Tax"), SpecialTileType.IncomeTax);

            tiles[5] = new RailroadTile(5, new RailroadTileOptions("Reading Railroad", 200, 100));
            tiles[6] = new PropertyTile(6, new PropertyTileOptions("Oriental Avenue", 100, 50, 50, 6, 30, 90, 270, 400, 550));
            tiles[7] = new SpecialTile(7, new TileOptions("Chance"), SpecialTileType.Chance);
            tiles[8] = new PropertyTile(8, new PropertyTileOptions("Vermont Avenue", 100, 50, 50, 6, 30, 90, 270, 400, 550));
            tiles[9] = new PropertyTile(9, new PropertyTileOptions("Connecticut Avenue", 120, 50, 60, 8, 40, 100, 300, 450, 600));

            tiles[10] = new SpecialTile(10, new TileOptions("Jail (Just visiting)"), SpecialTileType.VisitingJail);
            tiles[11] = new PropertyTile(11, new PropertyTileOptions("St. Charles Place", 140, 100, 70, 10, 50, 150, 450, 625, 750));
            tiles[12] = new UtilityTile(12, new UtilityTileOptions("Electric Company", 150, 75));
            tiles[13] = new PropertyTile(13, new PropertyTileOptions("States Avenue", 140, 100, 70, 10, 50, 150, 450, 625, 750));
            tiles[14] = new PropertyTile(14, new PropertyTileOptions("Virginia Avenue", 160, 100, 80, 12, 60, 180, 500, 700, 900));

            tiles[15] = new RailroadTile(15, new RailroadTileOptions("Pennsylvania Railroad", 200, 100));
            tiles[16] = new PropertyTile(16, new PropertyTileOptions("St. James Place", 180, 100, 90, 14, 70, 200, 550, 750, 950));
            tiles[17] = new SpecialTile(17, new TileOptions("Community Chest"), SpecialTileType.CommunityChest);
            tiles[18] = new PropertyTile(18, new PropertyTileOptions("Tennessee Avenue", 180, 100, 90, 14, 70, 200, 550, 750, 950));
            tiles[19] = new PropertyTile(19, new PropertyTileOptions("New York Avenue", 200, 100, 100, 16, 80, 220, 600, 800, 1000));

            tiles[20] = new SpecialTile(20, new TileOptions("Free Parking"), SpecialTileType.FreeParking);
            tiles[21] = new PropertyTile(21, new PropertyTileOptions("Kentucky Avenue", 220, 150, 110, 18, 90, 250, 700, 875, 1050));
            tiles[22] = new SpecialTile(22, new TileOptions("Chance"), SpecialTileType.Chance);
            tiles[23] = new PropertyTile(23, new PropertyTileOptions("Indiana Avenue", 220, 150, 110, 18, 90, 250, 700, 875, 1050));
            tiles[24] = new PropertyTile(24, new PropertyTileOptions("Illinois Avenue", 240, 150, 120, 20, 100, 300, 750, 925, 1100));

            tiles[25] = new RailroadTile(25, new RailroadTileOptions("B. & O. Railroad", 200, 100));
            tiles[26] = new PropertyTile(26, new PropertyTileOptions("Atlantic Avenue", 260, 150, 130, 22, 110, 330, 800, 975, 1150));
            tiles[27] = new PropertyTile(27, new PropertyTileOptions("Ventnor Avenue", 260, 150, 130, 22, 110, 330, 800, 975, 1150));
            tiles[28] = new UtilityTile(28, new UtilityTileOptions("Water Works", 150, 75));
            tiles[29] = new PropertyTile(29, new PropertyTileOptions("Marvin Gardens", 280, 150, 140, 24, 120, 330, 850, 1025, 1200));

            tiles[30] = new SpecialTile(30, new TileOptions("Go To Jail"), SpecialTileType.GoToJail);
            tiles[31] = new PropertyTile(31, new PropertyTileOptions("Pacific Avenue", 300, 200, 150, 26, 130, 390, 900, 1100, 1275));
            tiles[32] = new PropertyTile(32, new PropertyTileOptions("North Carolina Avenue", 300, 200, 150, 26, 130, 390, 900, 1100, 1275));
            tiles[33] = new SpecialTile(33, new TileOptions("Community Chest"), SpecialTileType.CommunityChest);
            tiles[34] = new PropertyTile(34, new PropertyTileOptions("Pennsylvania Avenue", 320, 200, 160, 28, 150, 450, 1000, 1200, 1400));

            tiles[35] = new RailroadTile(35, new RailroadTileOptions("Short Line", 200, 100));
            tiles[36] = new SpecialTile(36, new TileOptions("Chance"), SpecialTileType.Chance);
            tiles[37] = new PropertyTile(37, new PropertyTileOptions("Park Place", 350, 200, 175, 35, 175, 500, 1100, 1300, 1500));
            tiles[38] = new SpecialTile(38, new TileOptions("Luxury Tax"), SpecialTileType.LuxuryTax);
            tiles[39] = new PropertyTile(39, new PropertyTileOptions("Broadwalk", 400, 200, 200, 50, 200, 600, 1400, 1700, 2000));
        }

        public Tile GetTile(int position)
        {
            return tiles[position];
        }

        public int[] GetSpecialTileIndex(SpecialTileType type)
        {
            switch (type)
            {
                case SpecialTileType.GO:
                    return new int[] { 0 };
                case SpecialTileType.Chance:
                    return new int[] { 7, 22, 36 };
                case SpecialTileType.CommunityChest:
                    return new int[] { 2, 17, 33 };
                case SpecialTileType.VisitingJail:
                    return new int[] { 10 };
                case SpecialTileType.GoToJail:
                    return new int[] { 30 };
                case SpecialTileType.FreeParking:
                    return new int[] { 20 };
                case SpecialTileType.IncomeTax:
                    return new int[] { 4 };
                case SpecialTileType.LuxuryTax:
                    return new int[] { 38 };
                default:
                    throw new Exception($"Unhandled special tile type: {type}");
            }
        }
    }
    
    public enum SpecialTileType
    {
        GO,
        Chance,
        CommunityChest,
        VisitingJail,
        GoToJail,
        FreeParking,
        IncomeTax,
        LuxuryTax
    }
}
