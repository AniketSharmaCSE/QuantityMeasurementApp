using System;

namespace QuantityMeasurementApp
{
    public class QuantityMeasurementAppMain
    {
        public static void Main(string[] args)
        {
            // Example 1
            var q1 = new Quantity(1.0, LengthUnit.Feet);
            var q2 = new Quantity(12.0, LengthUnit.Inches);

            var checker = new EqualityChecker();
            Console.WriteLine(checker.CheckEquality(q1, q2));

            // Example 2
            var q3 = new Quantity(1.0, LengthUnit.Inches);
            var q4 = new Quantity(1.0, LengthUnit.Inches);

            Console.WriteLine(checker.CheckEquality(q3, q4));
        }
    }
}