using System;

namespace QuantityMeasurementApp
{
    public class EqualityChecker
    {
        public static bool AreFeetEqual(double first, double second)
        {
            Feet firstValue = new Feet(first);
            Feet secondValue = new Feet(second);

            return firstValue.Equals(secondValue);
        }

        public static bool AreInchesEqual(double first, double second)
        {
            Inches firstValue = new Inches(first);
            Inches secondValue = new Inches(second);

            return firstValue.Equals(secondValue);
        }
    }
}