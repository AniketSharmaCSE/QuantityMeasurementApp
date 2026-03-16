  
  namespace QuantityMeasurement.Model.Units{

  public static class LengthUnitExtensions
    {
        public static double GetFactor(this LengthUnit unit)
        {
            switch (unit)
            {
                case LengthUnit.Feet: 
                    return 1.0;
                case LengthUnit.Inches: 
                    return 1.0 / 12.0;
                case LengthUnit.Yards: 
                    return 3.0;
                case LengthUnit.Centimeters: 
                    return 1.0 / 30.48;
                default: 
                    throw new ArgumentException("Invalid unit");
            }
        }

        public static double ConvertToBaseUnit(this LengthUnit unit, double value)
        {
            return value * unit.GetFactor();
        }

        public static double ConvertFromBaseUnit(this LengthUnit unit, double baseValue)
        {
            return baseValue / unit.GetFactor();
        }

        public static string GetUnitName(this LengthUnit unit)
        {
            return unit.ToString();
        }
    }
  }