using System;

namespace QuantityMeasurementApp
{
    class QuantityMeasurementAppMain
    {
        static void Main(string[] args)
        {
            RunFeetEqualityUserInput();      // UC1
            RunInchesEqualityHardcoded();    // UC2
        }

        static void RunFeetEqualityUserInput()
        {
            // UC1 – Feet equality using user input

            Console.WriteLine("Enter first feet value:");
            string input1 = Console.ReadLine();

            Console.WriteLine("Enter second feet value:");
            string input2 = Console.ReadLine();

            if (!double.TryParse(input1, out double value1) ||
                !double.TryParse(input2, out double value2))
            {
                Console.WriteLine("Invalid numeric input.");
                return;
            }

            Feet feet1 = new Feet(value1);
            Feet feet2 = new Feet(value2);

            bool result = feet1.Equals(feet2);

            Console.WriteLine($"Feet Equal: {result}");
            Console.WriteLine();
        }

        static void RunInchesEqualityHardcoded()
        {
            // UC2 – Inches equality using hardcoded values

            Inches inch1 = new Inches(1.0);
            Inches inch2 = new Inches(1.0);

            bool result = inch1.Equals(inch2);

            Console.WriteLine($"Inches Equal: {result}");
        }
    }
}