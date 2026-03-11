using System;
using QuantityMeasurementApp.Units;

namespace QuantityMeasurementApp.Models
{
    public class Quantity<U>
    {
        private readonly double value;
        private readonly U unit;

        public Quantity(double value, U unit)
        {
            if (unit == null)
            {
                throw new ArgumentException("Unit cannot be null.");
            }

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

        public U GetUnit()
        {
            return unit;
        }

        private double ConvertToBase(double val, U u)
        {
            if (u is LengthUnit)
            {
                return LengthUnitExtensions.ConvertToBaseUnit((LengthUnit)(object)u, val);
            }

            if (u is WeightUnit)
            {
                return WeightUnitExtensions.ConvertToBaseUnit((WeightUnit)(object)u, val);
            }

            if (u is VolumeUnit)
            {
                return VolumeUnitExtensions.ConvertToBaseUnit((VolumeUnit)(object)u, val);
            }

            throw new ArgumentException("Unsupported unit type");
        }

        private double ConvertFromBase(double baseVal, U u)
        {
            if (u is LengthUnit)
            {
                return LengthUnitExtensions.ConvertFromBaseUnit((LengthUnit)(object)u, baseVal);
            }

            if (u is WeightUnit)
            {
                return WeightUnitExtensions.ConvertFromBaseUnit((WeightUnit)(object)u, baseVal);
            }

            if (u is VolumeUnit)
            {
                return VolumeUnitExtensions.ConvertFromBaseUnit((VolumeUnit)(object)u, baseVal);
            }

            throw new ArgumentException("Unsupported unit type");
        }

        public Quantity<U> ConvertTo(U targetUnit)
        {
            double baseValue = ConvertToBase(value, unit);
            double converted = ConvertFromBase(baseValue, targetUnit);

            return new Quantity<U>(converted, targetUnit);
        }

        public Quantity<U> Add(Quantity<U> other)
        {
            if (other == null)
            {
                throw new ArgumentException("Second quantity cannot be null.");
            }

            double base1 = ConvertToBase(value, unit);
            double base2 = ConvertToBase(other.value, other.unit);

            double sum = base1 + base2;

            double result = ConvertFromBase(sum, unit);

            return new Quantity<U>(result, unit);
        }

        public Quantity<U> Add(Quantity<U> other, U targetUnit)
        {
            if (other == null)
            {
                throw new ArgumentException("Second quantity cannot be null.");
            }

            double base1 = ConvertToBase(value, unit);
            double base2 = ConvertToBase(other.value, other.unit);

            double sum = base1 + base2;

            double result = ConvertFromBase(sum, targetUnit);

            return new Quantity<U>(result, targetUnit);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Quantity<U>))
            {
                return false;
            }

            Quantity<U> other = (Quantity<U>)obj;

            double base1 = ConvertToBase(value, unit);
            double base2 = ConvertToBase(other.value, other.unit);

            return Math.Abs(base1 - base2) < 0.0001;
        }

        public override int GetHashCode()
        {
            double baseValue = ConvertToBase(value, unit);
            return baseValue.GetHashCode();
        }
    }
}