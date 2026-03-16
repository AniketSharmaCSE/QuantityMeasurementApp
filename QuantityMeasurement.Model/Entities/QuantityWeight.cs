using System;

namespace QuantityMeasurement.Model.Entities
{
    public class QuantityWeight
    {
        private readonly double value;
        private readonly string unit;

        public QuantityWeight(double value, string unit)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                throw new ArgumentException("Invalid numeric value.");
            }

            this.value = value;
            this.unit = unit;
        }

        public double GetValue()
        {
            return value;
        }

        public string GetUnit()
        {
            return unit;
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is QuantityWeight))
            {
                return false;
            }

            QuantityWeight other = (QuantityWeight)obj;

            return value == other.value && unit == other.unit;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(value, unit);
        }
    }
}