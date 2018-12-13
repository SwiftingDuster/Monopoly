namespace Monopoly
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Monopoly monopoly = new Monopoly(0, 6);
            monopoly.SetUp().Start();
        }
    }
}
