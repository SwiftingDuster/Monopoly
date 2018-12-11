namespace Monopoly
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Monopoly monopoly = new Monopoly(1, 3);
            monopoly.SetUp().Start();
        }
    }
}
