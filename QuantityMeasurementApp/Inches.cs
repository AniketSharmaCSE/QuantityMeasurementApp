using System;

namespace QuantityMeasurementApp
{
    public class Inches
    {
        private double value;

        public Inches(double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                throw new ArgumentException("Invalid numeric value");
            }

            this.value = value;
        }

        public double GetValue()
        {
            return value;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (!(obj is Inches))
                return false;

            Inches other = (Inches)obj;

            return this.value == other.value;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }
}