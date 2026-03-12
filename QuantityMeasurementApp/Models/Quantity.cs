using System;
using QuantityMeasurementApp.Units;

namespace QuantityMeasurementApp.Models
{
    public class Quantity<U>
    {
        private readonly double value;
        private readonly U unit;

        private enum ArithmeticOperation
        {
            ADD,
            SUBTRACT,
            DIVIDE
        }

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
            if (u is LengthUnit lu)
                return LengthUnitExtensions.ConvertToBaseUnit(lu, val);

            if (u is WeightUnit wu)
                return WeightUnitExtensions.ConvertToBaseUnit(wu, val);

            if (u is VolumeUnit vu)
                return VolumeUnitExtensions.ConvertToBaseUnit(vu, val);

            // UC14 support
            if (u is TemperatureUnit tu)
                return tu.ConvertToBase(val);

            throw new ArgumentException("Unsupported unit type");
        }

        private double ConvertFromBase(double baseVal, U u)
        {
            if (u is LengthUnit lu)
                return LengthUnitExtensions.ConvertFromBaseUnit(lu, baseVal);

            if (u is WeightUnit wu)
                return WeightUnitExtensions.ConvertFromBaseUnit(wu, baseVal);

            if (u is VolumeUnit vu)
                return VolumeUnitExtensions.ConvertFromBaseUnit(vu, baseVal);

            // UC14 support
            if (u is TemperatureUnit tu)
                return tu.ConvertFromBase(baseVal);

            throw new ArgumentException("Unsupported unit type");
        }

        // centralized arithmetic helper
        private double performBaseArithmetic(Quantity<U> other, ArithmeticOperation op)
        {
            if (other == null)
            {
                throw new ArgumentException("Second quantity cannot be null.");
            }

            double base1 = ConvertToBase(this.value, this.unit);
            double base2 = ConvertToBase(other.value, other.unit);

            switch (op)
            {
                case ArithmeticOperation.ADD:
                    return base1 + base2;

                case ArithmeticOperation.SUBTRACT:
                    return base1 - base2;

                case ArithmeticOperation.DIVIDE:
                    if (base2 == 0)
                    {
                        throw new ArithmeticException("Cannot divide by zero.");
                    }
                    return base1 / base2;
            }

            throw new ArgumentException("Invalid arithmetic operation");
        }

        public Quantity<U> Add(Quantity<U> other)
        {
            if (unit is IMeasurable m)
                m.ValidateOperationSupport("ADD");

            double baseResult = performBaseArithmetic(other, ArithmeticOperation.ADD);
            double result = ConvertFromBase(baseResult, this.unit);

            return new Quantity<U>(result, this.unit);
        }

        public Quantity<U> Add(Quantity<U> other, U targetUnit)
        {
            if (unit is IMeasurable m)
                m.ValidateOperationSupport("ADD");

            double baseResult = performBaseArithmetic(other, ArithmeticOperation.ADD);
            double result = ConvertFromBase(baseResult, targetUnit);

            return new Quantity<U>(result, targetUnit);
        }

        public Quantity<U> Subtract(Quantity<U> other)
        {
            if (unit is IMeasurable m)
                m.ValidateOperationSupport("SUBTRACT");

            double baseResult = performBaseArithmetic(other, ArithmeticOperation.SUBTRACT);
            double result = ConvertFromBase(baseResult, this.unit);

            return new Quantity<U>(result, this.unit);
        }

        public Quantity<U> Subtract(Quantity<U> other, U targetUnit)
        {
            if (unit is IMeasurable m)
                m.ValidateOperationSupport("SUBTRACT");

            double baseResult = performBaseArithmetic(other, ArithmeticOperation.SUBTRACT);
            double result = ConvertFromBase(baseResult, targetUnit);

            return new Quantity<U>(result, targetUnit);
        }

        public double Divide(Quantity<U> other)
        {
            if (unit is IMeasurable m)
                m.ValidateOperationSupport("DIVIDE");

            return performBaseArithmetic(other, ArithmeticOperation.DIVIDE);
        }

        public Quantity<U> ConvertTo(U targetUnit)
        {
            double baseValue = ConvertToBase(value, unit);
            double converted = ConvertFromBase(baseValue, targetUnit);

            return new Quantity<U>(converted, targetUnit);
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is Quantity<U> other))
            {
                return false;
            }

            double base1 = ConvertToBase(this.value, this.unit);
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