namespace QuantityMeasurementApp;

internal static class QuantityMeasurementAppMain
{
    public static void Main(string[] args)
    {
        Console.Write("Enter first value in feet: ");
        string firstInput = Console.ReadLine();
        Console.Write("Enter second value in feet: ");
        string secondInput = Console.ReadLine();

        bool firstOk = double.TryParse(firstInput, out double num1);
        bool secondOk = double.TryParse(secondInput, out double num2);

        if (firstOk == false || secondOk == false)
        {
            Console.WriteLine("Invalid input. Please enter numeric values.");
            return;
        }

        EqualityChecker app = new EqualityChecker();
        Feet feet1 = new Feet(num1);
        Feet feet2 = new Feet(num2);

        bool result = app.AreEqual(feet1, feet2);

        Console.WriteLine("Input: " + num1 + " ft and " + num2 + " ft");
        Console.WriteLine("Output: Equal (" + result.ToString().ToLowerInvariant() + ")");
    }
}
