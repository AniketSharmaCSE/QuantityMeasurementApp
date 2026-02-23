using System;

namespace QuantityMeasurementApp
{
    public class QuantityMeasurementAppMain
    {
        public static void Main(string[] args)
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.Feet);
            Quantity q2 = new Quantity(12.0, LengthUnit.Inches);

            EqualityChecker checker = new EqualityChecker();
            Console.WriteLine(checker.CheckEquality(q1, q2));

            Quantity q3 = new Quantity(1.0, LengthUnit.Inches);
            Quantity q4 = new Quantity(1.0, LengthUnit.Inches);

            Console.WriteLine(checker.CheckEquality(q3, q4));
        }
    }
}