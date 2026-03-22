using QuantityMeasurement.ConsoleApp.UI;

namespace QuantityMeasurement.ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            IMenu menu = new Menu();
            menu.Show();
        }
    }
}