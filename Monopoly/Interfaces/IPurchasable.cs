namespace Monopoly
{
    public interface IPurchasable
    {
        Player Owner { get; }
        bool CanPurchase(Player player);
        void Purchase(Player player);
        void Disown();
    }
}