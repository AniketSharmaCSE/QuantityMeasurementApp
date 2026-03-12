using System;

namespace QuantityMeasurementApp.Units
{
    public interface IMeasurable
    {
        double ConvertToBase(double value);

        double ConvertFromBase(double baseValue);

        bool SupportsArithmetic();

        void ValidateOperationSupport(string operation);
    }
}