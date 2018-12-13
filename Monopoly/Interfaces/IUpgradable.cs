namespace Monopoly
{
    public interface IUpgradable
    {
        int HouseCount { get; set; }

        bool CanUpgrade(Player player);
        void Upgrade();
        int SellUpgrade(int priceCut = 2);
    }
}
