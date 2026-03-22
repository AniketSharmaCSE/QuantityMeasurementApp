using QuantityMeasurement.Model.DTOs;

namespace QuantityMeasurement.Repository.Interfaces
{
    public interface IQuantityMeasurementRepository
    {
        void Save(QuantityResponseDTO response);

        IReadOnlyList<QuantityResponseDTO> GetAll();
        IReadOnlyList<QuantityResponseDTO> GetRecent(int count);
        IReadOnlyList<QuantityResponseDTO> GetByOperation(string operation);
        IReadOnlyList<QuantityResponseDTO> GetByCategory(string category);

        int GetTotalCount();
        string GetPoolStatistics();

        // remove all records
        void Clear();

        // alias used by the console menu
        void DeleteAll() => Clear();

        void ReleaseResources();
    }
}
