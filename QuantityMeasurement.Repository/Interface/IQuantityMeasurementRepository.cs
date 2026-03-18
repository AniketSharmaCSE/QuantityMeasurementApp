using QuantityMeasurement.Model.DTOs;

namespace QuantityMeasurement.Repository.Interfaces
{
    public interface IQuantityMeasurementRepository
    {
        void Save(QuantityResponseDTO response);

        IReadOnlyList<QuantityResponseDTO> GetAll();

        void Clear();

        // filter by operation name, e.g. "Add", "Convert"
        IReadOnlyList<QuantityResponseDTO> GetByOperation(string operation);

        // filter by measurement category, e.g. "Length", "Weight"
        IReadOnlyList<QuantityResponseDTO> GetByCategory(string category);

        int GetTotalCount();

        // overridden by the database repo to return live pool stats;
        // the cache repo just returns a placeholder
        string GetPoolStatistics() => "N/A in-memory repository has no connection pool.";

        // overridden by the database repo to close connections on shutdown;
        // no-op for the cache repo
        void ReleaseResources() { }
    }
}
