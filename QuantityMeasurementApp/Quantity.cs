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

        // Static API required in UC5
        public static double Convert(double value, LengthUnit source, LengthUnit target)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                throw new ArgumentException("Invalid numeric value.");
            }

            Quantity temp = new Quantity(value, source);
            Quantity converted = temp.ConvertTo(target);

            return converted.value;
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

        // UC5: Convert current quantity to target unit
        public Quantity ConvertTo(LengthUnit targetUnit)
        {
            double valueInBase = ConvertToBase();
            double convertedValue = 0;

            switch (targetUnit)
            {
                case LengthUnit.Feet:
                    convertedValue = valueInBase / 12.0;
                    break;

                case LengthUnit.Inches:
                    convertedValue = valueInBase;
                    break;

                case LengthUnit.Yards:
                    convertedValue = valueInBase / 36.0;
                    break;

                case LengthUnit.Centimeters:
                    convertedValue = valueInBase / 0.393701;
                    break;

                default:
                    throw new ArgumentException("Invalid target unit");
            }

            return new Quantity(convertedValue, targetUnit);
        }

        // UC6: Add two length quantities
        public Quantity Add(Quantity other)
        {
            if (other == null)
            {
                throw new ArgumentException("Second quantity cannot be null.");
            }

            // convert both to base unit (inches)
            double firstInBase = this.ConvertToBase();
            double secondInBase = other.ConvertToBase();

            // add in base unit
            double sumInBase = firstInBase + secondInBase;

            // convert result to unit of first operand
            Quantity resultInTarget = new Quantity(sumInBase, LengthUnit.Inches)
                                        .ConvertTo(this.unit);

            return resultInTarget;
        }

        // UC7: Addition with target unit
        public Quantity Add(Quantity other, LengthUnit targetUnit)
        {
            if (other == null)
            {
                throw new ArgumentException("Second quantity cannot be null.");
            }

            double firstInBase = this.ConvertToBase();
            double secondInBase = other.ConvertToBase();

            double sumInBase = firstInBase + secondInBase;

            Quantity result = new Quantity(sumInBase, LengthUnit.Inches).ConvertTo(targetUnit);

            return result;
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

            return Math.Abs(this.ConvertToBase() - other.ConvertToBase()) < 0.0001;
        }

        public override int GetHashCode()
        {
            return ConvertToBase().GetHashCode();
        }
    }
}