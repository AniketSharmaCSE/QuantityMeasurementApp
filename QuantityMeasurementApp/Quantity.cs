using System;

namespace QuantityMeasurementApp
{
    public class Quantity
    {
        private readonly double value;
        private readonly LengthUnit unit;

        public Quantity(double value, LengthUnit unit)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                throw new ArgumentException("Invalid numeric value.");  
            }

            this.value = value;
            this.unit = unit;
        }

        public static double Convert(double value, LengthUnit source, LengthUnit target)
        {
            double baseValue = source.ConvertToBaseUnit(value);
            return target.ConvertFromBaseUnit(baseValue);
        }

        public Quantity ConvertTo(LengthUnit targetUnit)
        {
            double baseValue = unit.ConvertToBaseUnit(value);
            double convertedValue = targetUnit.ConvertFromBaseUnit(baseValue);

            return new Quantity(convertedValue, targetUnit);
        }

        public Quantity Add(Quantity other)
        {
            if (other == null)
            {
                throw new ArgumentException("Second quantity cannot be null.");
            }

            double firstBase = unit.ConvertToBaseUnit(value);
            double secondBase = other.unit.ConvertToBaseUnit(other.value);

            double sumBase = firstBase + secondBase;

            double result = unit.ConvertFromBaseUnit(sumBase);

            return new Quantity(result, unit);
        }

        public Quantity Add(Quantity other, LengthUnit targetUnit)
        {
            if (other == null)
            {
                throw new ArgumentException("Second quantity cannot be null.");
            }

            double firstBase = unit.ConvertToBaseUnit(value);
            double secondBase = other.unit.ConvertToBaseUnit(other.value);

            double sumBase = firstBase + secondBase;

            double result = targetUnit.ConvertFromBaseUnit(sumBase);

            return new Quantity(result, targetUnit);
        }

        public double GetValue()
        {
            return value;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Quantity))
            {
                return false;
            }

            Quantity other = (Quantity)obj;

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