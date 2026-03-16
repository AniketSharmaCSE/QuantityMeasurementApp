using System;

namespace QuantityMeasurement.Model.Interfaces
{
    public interface IMeasurable
    {
        double ConvertToBase(double value);

        double ConvertFromBase(double baseValue);

        bool SupportsArithmetic();

        void ValidateOperationSupport(string operation);
    }
}