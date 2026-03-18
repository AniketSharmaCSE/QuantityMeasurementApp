using System;
using QuantityMeasurement.Model.Interfaces;

namespace QuantityMeasurement.Model.Units{
    public class TemperatureUnit : IMeasurable
    {
        private string name;

        private TemperatureUnit(string name)
        {
            this.name = name;
        }

        public static readonly TemperatureUnit Celsius = new TemperatureUnit("Celsius");
        public static readonly TemperatureUnit Fahrenheit = new TemperatureUnit("Fahrenheit");
        public static readonly TemperatureUnit Kelvin = new TemperatureUnit("Kelvin");

        public double ConvertToBase(double value)
        {
            if (this == Celsius)
                return value;

            if (this == Fahrenheit)
                return (value - 32) * 5 / 9;

            if (this == Kelvin)
                return value - 273.15;

            throw new ArgumentException("Invalid temperature unit");
        }

        public double ConvertFromBase(double baseValue)
        {
            if (this == Celsius)
                return baseValue;

            if (this == Fahrenheit)
                return (baseValue * 9 / 5) + 32;

            if (this == Kelvin)
                return baseValue + 273.15;

            throw new ArgumentException("Invalid temperature unit");
        }

        public bool SupportsArithmetic()
        {
            return false;
        }

        public void ValidateOperationSupport(string operation)
        {
            throw new NotSupportedException("Temperature does not support arithmetic operation: " + operation);
        }

        public override string ToString()
        {
            return name;
        }
    }
}