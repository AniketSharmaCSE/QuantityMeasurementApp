namespace QuantityMeasurement.Model.Units{

    public static class VolumeUnitExtensions
    {
        public static double GetFactor(this VolumeUnit unit)
        {
            switch (unit)
            {
                case VolumeUnit.Litre: return 1.0;
                case VolumeUnit.Millilitre: return 0.001;
                case VolumeUnit.Gallon: return 3.78541;
                default: throw new ArgumentException("Invalid unit");
            }
        }

        public static double ConvertToBaseUnit(this VolumeUnit unit, double value)
        {
            return value * unit.GetFactor();
        }

        public static double ConvertFromBaseUnit(this VolumeUnit unit, double baseValue)
        {
            return baseValue / unit.GetFactor();
        }

        public static string GetUnitName(this VolumeUnit unit)
        {
            return unit.ToString();
        }
    }
}