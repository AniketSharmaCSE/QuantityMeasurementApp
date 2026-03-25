using QuantityMeasurement.Model.DTOs;

namespace QuantityMeasurement.BusinessLayer.Interfaces
{
    public interface IQuantityService
    {
        // Length
        QuantityResponseDTO AddLength(double v1, string u1, double v2, string u2);
        QuantityResponseDTO AddLengthWithTarget(double v1, string u1, double v2, string u2, string targetUnit);
        QuantityResponseDTO SubtractLength(double v1, string u1, double v2, string u2);
        QuantityResponseDTO DivideLength(double v1, string u1, double v2, string u2);
        QuantityResponseDTO ConvertLength(double value, string fromUnit, string toUnit);
        QuantityResponseDTO CompareLength(double v1, string u1, double v2, string u2);

        // Weight
        QuantityResponseDTO AddWeight(double v1, string u1, double v2, string u2);
        QuantityResponseDTO SubtractWeight(double v1, string u1, double v2, string u2);
        QuantityResponseDTO DivideWeight(double v1, string u1, double v2, string u2);
        QuantityResponseDTO ConvertWeight(double value, string fromUnit, string toUnit);
        QuantityResponseDTO CompareWeight(double v1, string u1, double v2, string u2);

        // Volume
        QuantityResponseDTO AddVolume(double v1, string u1, double v2, string u2);
        QuantityResponseDTO SubtractVolume(double v1, string u1, double v2, string u2);
        QuantityResponseDTO DivideVolume(double v1, string u1, double v2, string u2);
        QuantityResponseDTO ConvertVolume(double value, string fromUnit, string toUnit);
        QuantityResponseDTO CompareVolume(double v1, string u1, double v2, string u2);

        // Temperature
        QuantityResponseDTO ConvertTemperature(double value, string fromUnit, string toUnit);
        QuantityResponseDTO CompareTemperature(double v1, string u1, double v2, string u2);

        // History - moved from HistoryController to keep repo out of API layer
        IReadOnlyList<QuantityResponseDTO> GetHistory();
        IReadOnlyList<QuantityResponseDTO> GetRecentHistory(int count);
        IReadOnlyList<QuantityResponseDTO> GetHistoryByOperation(string operation);
        IReadOnlyList<QuantityResponseDTO> GetHistoryByCategory(string category);
        int GetTotalCount();
        Dictionary<string, int> GetOperationStats();
        void ClearHistory();
    }
}
