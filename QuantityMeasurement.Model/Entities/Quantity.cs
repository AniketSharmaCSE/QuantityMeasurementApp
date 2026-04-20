using QuantityMeasurement.Model.Units;

namespace QuantityMeasurement.Model.Entities
{
    // generic quantity that holds a value and its unit
    // T is one of the unit enums: LengthUnit, WeightUnit, VolumeUnit, TemperatureUnit
    public class Quantity<T> where T : struct, Enum
    {
        private readonly double _value;
        private readonly T _unit;

        // conversion factors to a base unit for each enum type
        // LengthUnit base = Feet, WeightUnit base = Gram, VolumeUnit base = Litre
        private static readonly Dictionary<string, double> ToBase = new()
        {
            // LengthUnit -> Feet as base
            { "Feet",        1.0 },
            { "Inches",      1.0 / 12.0 },
            { "Yards",       3.0 },
            { "Centimeters", 0.032808399 },

            // WeightUnit -> Gram as base
            { "Gram",        1.0 },
            { "Kilogram",    1000.0 },
            { "Pound",       453.592 },

            // VolumeUnit -> Litre as base
            { "Litre",       1.0 },
            { "Millilitre",  0.001 },
            { "Gallon",      3.785 },

            // TemperatureUnit handled separately below (non-linear)
            { "Celsius",     1.0 },
            { "Fahrenheit",  1.0 },
            { "Kelvin",      1.0 }
        };

        public Quantity(double value, T unit)
        {
            _value = value;
            _unit  = unit;
        }

        public double GetValue() => _value;
        public T GetUnit() => _unit;

        private double ToBaseValue()
        {
            string name = _unit.ToString()!;

            // temperature is non-linear so convert to Celsius first
            if (_unit is Units.TemperatureUnit tu)
            {
                return tu switch
                {
                    Units.TemperatureUnit.Celsius    => _value,
                    Units.TemperatureUnit.Fahrenheit => (_value - 32) * 5.0 / 9.0,
                    Units.TemperatureUnit.Kelvin     => _value - 273.15,
                    _ => throw new ArgumentException("Unknown temperature unit")
                };
            }

            return _value * ToBase[name];
        }

        public Quantity<T> ConvertTo(T targetUnit)
        {
            if (_unit is Units.TemperatureUnit)
            {
                double celsius = ToBaseValue();

                var tu = (Units.TemperatureUnit)(object)targetUnit;
                double result = tu switch
                {
                    Units.TemperatureUnit.Celsius    => celsius,
                    Units.TemperatureUnit.Fahrenheit => celsius * 9.0 / 5.0 + 32,
                    Units.TemperatureUnit.Kelvin     => celsius + 273.15,
                    _ => throw new ArgumentException("Unknown temperature unit")
                };

                return new Quantity<T>(result, targetUnit);
            }

            double baseVal    = ToBaseValue();
            double targetBase = ToBase[targetUnit.ToString()!];
            return new Quantity<T>(baseVal / targetBase, targetUnit);
        }

        public Quantity<T> Add(Quantity<T> other)
        {
            double resultInBase = ToBaseValue() + other.ToBaseValue();
            double targetBase   = ToBase[_unit.ToString()!];
            return new Quantity<T>(resultInBase / targetBase, _unit);
        }

        public Quantity<T> Add(Quantity<T> other, T targetUnit)
        {
            double resultInBase = ToBaseValue() + other.ToBaseValue();
            double targetBase   = ToBase[targetUnit.ToString()!];
            return new Quantity<T>(resultInBase / targetBase, targetUnit);
        }

        public Quantity<T> Subtract(Quantity<T> other)
        {
            double resultInBase = ToBaseValue() - other.ToBaseValue();
            double targetBase   = ToBase[_unit.ToString()!];
            return new Quantity<T>(resultInBase / targetBase, _unit);
        }

        // returns a plain scalar ratio (no unit)
        public double Divide(Quantity<T> other)
        {
            if (other.ToBaseValue() == 0)
                throw new DivideByZeroException("Cannot divide by zero.");

            return ToBaseValue() / other.ToBaseValue();
        }

        public override bool Equals(object? obj)
        {
            if (obj is Quantity<T> other)
                return Math.Abs(ToBaseValue() - other.ToBaseValue()) < 1e-9;
            return false;
        }

        public override int GetHashCode() => ToBaseValue().GetHashCode();
    }
}
