using System;

namespace QuantityMeasurementApp.Units
{
    public enum LengthUnit
    {
        Feet,
        Inches,
        Yards,
        Centimeters
    }

    public static class LengthUnitExtensions
    {
        public static double ConvertToBaseUnit(this LengthUnit unit, double value)
        {
            switch (unit)
            {
                case LengthUnit.Feet:
                    return value;

                case LengthUnit.Inches:
                    return value / 12.0;

                case LengthUnit.Yards:
                    return value * 3.0;

                case LengthUnit.Centimeters:
                    return value / 30.48;

                default:
                    throw new ArgumentException("Invalid unit");
            }
        }

        public static double ConvertFromBaseUnit(this LengthUnit unit, double baseValue)
        {
            switch (unit)
            {
                case LengthUnit.Feet:
                    return baseValue;

                case LengthUnit.Inches:
                    return baseValue * 12.0;

                case LengthUnit.Yards:
                    return baseValue / 3.0;

                case LengthUnit.Centimeters:
                    return baseValue * 30.48;

                default:
                    throw new ArgumentException("Invalid unit");
            }
        }
    }
}