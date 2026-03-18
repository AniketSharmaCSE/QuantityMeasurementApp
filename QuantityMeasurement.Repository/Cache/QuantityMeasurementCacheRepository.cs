using QuantityMeasurement.Repository.Interfaces;
using QuantityMeasurement.Model.DTOs;

namespace QuantityMeasurement.Repository
{
    public class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
    {
        private readonly List<QuantityResponseDTO> _cache = new();

        public void Save(QuantityResponseDTO response)
        {
            _cache.Add(response);
        }

        public IReadOnlyList<QuantityResponseDTO> GetAll()
        {
            return _cache.AsReadOnly();
        }

        public void Clear()
        {
            _cache.Clear();
        }

        public IReadOnlyList<QuantityResponseDTO> GetByOperation(string operation)
        {
            return _cache
                .Where(r => r.Operation.Equals(operation, StringComparison.OrdinalIgnoreCase))
                .ToList()
                .AsReadOnly();
        }

        public IReadOnlyList<QuantityResponseDTO> GetByCategory(string category)
        {
            // check both operands since some operations (e.g. Add) have two operands in the same category
            return _cache
                .Where(r =>
                    (r.Operand1?.Category.Equals(category, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (r.Operand2?.Category.Equals(category, StringComparison.OrdinalIgnoreCase) ?? false))
                .ToList()
                .AsReadOnly();
        }

        public int GetTotalCount()
        {
            return _cache.Count;
        }
    }
}
