using System;

namespace QuantityMeasurementApp
{
    public class Quantity
    {
        private readonly double value;
        private readonly LengthUnit unit;

        public Quantity(double value, LengthUnit unit)
        {
            if (unit == null)
            {
                throw new ArgumentNullException(nameof(unit));
            }

            this.value = value;
            this.unit = unit;
        }

        // Method to convert to base unit (inches)
        private double ConvertToBase()
        {
            switch (unit)
            {
                case LengthUnit.Feet:
                    return value * 12.0;

                case LengthUnit.Inches:
                    return value;

                case LengthUnit.Yards:
                    return value * 36.0;

                case LengthUnit.Centimeters:
                    return value * 0.393701;

                default:
                    throw new ArgumentException("Invalid unit");
            }
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is Quantity))
            {
                return false;
            }

            Quantity other = (Quantity)obj;

            // Using tolerance to avoid floating point precision issues
            return Math.Abs(this.ConvertToBase() - other.ConvertToBase()) < 0.0001;
        }

        public override int GetHashCode()
        {
            return ConvertToBase().GetHashCode();
        }
    }
}