using System;

namespace QuantityMeasurementApp
{
    public class Quantity
    {
        public double Value { get; }
        public LengthUnit Unit { get; }

        public Quantity(double value, LengthUnit unit)
        {
            if (value < 0)
            {
                throw new ArgumentException("Value cannot be negative.");
            }

            Unit = unit;
            Value = value;
        }

        private double ConvertToFeet()
        {
            return Value / (double)Unit;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj == null || obj.GetType() != typeof(Quantity))
            {
                return false;
            }

            Quantity other = (Quantity)obj;

            return Math.Abs(this.ConvertToFeet() - other.ConvertToFeet()) < 0.0001;
        }

        public override int GetHashCode()
        {
            return ConvertToFeet().GetHashCode();
        }
    }
}