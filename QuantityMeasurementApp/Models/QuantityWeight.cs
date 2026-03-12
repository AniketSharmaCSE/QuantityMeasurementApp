using System;
using QuantityMeasurementApp.Interfaces;
using QuantityMeasurementApp.Units;

namespace QuantityMeasurementApp.Models
{
    public class QuantityWeight : IQuantity
    {
        private readonly double value;
        private readonly WeightUnit unit;

        public QuantityWeight(double value, WeightUnit unit)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                throw new ArgumentException("Invalid numeric value.");
            }

            this.value = value;
            this.unit = unit;
        }

        public QuantityWeight ConvertTo(WeightUnit targetUnit)
        {
            double baseValue = unit.ConvertToBaseUnit(value);
            double convertedValue = targetUnit.ConvertFromBaseUnit(baseValue);

            return new QuantityWeight(convertedValue, targetUnit);
        }

        public QuantityWeight Add(QuantityWeight other)
        {
            if (other == null)
            {
                throw new ArgumentException("Second quantity cannot be null.");
            }

            double firstBase = unit.ConvertToBaseUnit(value);
            double secondBase = other.unit.ConvertToBaseUnit(other.value);

            double sumBase = firstBase + secondBase;

            double result = unit.ConvertFromBaseUnit(sumBase);

            return new QuantityWeight(result, unit);
        }

        public QuantityWeight Add(QuantityWeight other, WeightUnit targetUnit)
        {
            if (other == null)
            {
                throw new ArgumentException("Second quantity cannot be null.");
            }

            double firstBase = unit.ConvertToBaseUnit(value);
            double secondBase = other.unit.ConvertToBaseUnit(other.value);

            double sumBase = firstBase + secondBase;

            double result = targetUnit.ConvertFromBaseUnit(sumBase);

            return new QuantityWeight(result, targetUnit);
        }

        public double GetValue()
        {
            return value;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is QuantityWeight))
            {
                return false;
            }

            QuantityWeight other = (QuantityWeight)obj;

            double firstBase = unit.ConvertToBaseUnit(value);
            double secondBase = other.unit.ConvertToBaseUnit(other.value);

            return Math.Abs(firstBase - secondBase) < 0.0001;
        }

        public override int GetHashCode()
        {
            return unit.ConvertToBaseUnit(value).GetHashCode();
        }
    }
}