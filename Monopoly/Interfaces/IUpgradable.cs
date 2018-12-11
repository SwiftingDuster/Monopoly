namespace Monopoly
{
    public interface IUpgradable
    {
        bool CanUpgrade(Player player);
        void Upgrade();
    }
}
