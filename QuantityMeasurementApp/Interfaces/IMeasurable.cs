using System;

namespace QuantityMeasurementApp.Interfaces
{
    // Functional interface to indicate if arithmetic is supported
    public delegate bool SupportsArithmetic();

    public interface IMeasurable
    {
        double ConvertToBaseUnit(double value);

        double ConvertFromBaseUnit(double baseValue);

        string GetUnitName();

        // By default arithmetic is supported
        public bool SupportsArithmeticOperation()
        {
            SupportsArithmetic supportsArithmetic = () => true;
            return supportsArithmetic();
        }

        // Default validation method
        public void ValidateOperationSupport(string operation)
        {
            
        }
    }
}