using QuantityMeasurement.Model.DTOs;
using QuantityMeasurement.Repository.Interfaces;

namespace QuantityMeasurement.Repository
{
    // in-memory repository - no external dependencies, used when App:RepositoryType = "cache"
    public class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
    {
        private readonly List<QuantityResponseDTO> _store = new();
        private readonly object _lock = new();

        public void Save(QuantityResponseDTO response)
        {
            lock (_lock)
                _store.Add(response);
        }

        public IReadOnlyList<QuantityResponseDTO> GetAll()
        {
            lock (_lock)
                return _store.AsReadOnly();
        }

        public IReadOnlyList<QuantityResponseDTO> GetRecent(int count)
        {
            lock (_lock)
                return _store.TakeLast(count).Reverse().ToList().AsReadOnly();
        }

        public IReadOnlyList<QuantityResponseDTO> GetByOperation(string operation)
        {
            lock (_lock)
                return _store
                    .Where(r => r.Operation.Equals(operation, StringComparison.OrdinalIgnoreCase))
                    .ToList()
                    .AsReadOnly();
        }

        public IReadOnlyList<QuantityResponseDTO> GetByCategory(string category)
        {
            lock (_lock)
                return _store
                    .Where(r =>
                        (r.Operand1?.Category ?? "").Equals(category, StringComparison.OrdinalIgnoreCase) ||
                        (r.Result?.Category   ?? "").Equals(category, StringComparison.OrdinalIgnoreCase))
                    .ToList()
                    .AsReadOnly();
        }

        public int GetTotalCount()
        {
            lock (_lock)
                return _store.Count;
        }

        public string GetPoolStatistics() => "Cache mode - no connection pool";

        public void Clear()
        {
            lock (_lock)
                _store.Clear();
        }

        public void ReleaseResources() { }
    }
}
