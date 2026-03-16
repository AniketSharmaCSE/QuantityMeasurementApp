using QuantityMeasurement.Repository.Interfaces;
using QuantityMeasurement.Model.DTOs;

namespace QuantityMeasurement.Repository
{
    public class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
    {
        private readonly List<QuantityResponseDTO> cache = new();

        public void Save(QuantityResponseDTO response)
        {
            cache.Add(response);
        }

        public IReadOnlyList<QuantityResponseDTO> GetAll()
        {
            return cache.AsReadOnly();
        }

        public void Clear()
        {
            cache.Clear();
        }
    }
}