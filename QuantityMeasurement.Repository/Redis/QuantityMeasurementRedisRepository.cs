using System.Text.Json;
using QuantityMeasurement.Model.DTOs;
using QuantityMeasurement.Repository.Interfaces;
using StackExchange.Redis;

namespace QuantityMeasurement.Repository.Redis
{
    public class QuantityMeasurementRedisRepository : IQuantityMeasurementRepository
    {
        private readonly IDatabase _db;
        private const string ListKey = "qty:history";

        public QuantityMeasurementRedisRepository(string connectionString)
        {
            var mux = ConnectionMultiplexer.Connect(connectionString);
            _db = mux.GetDatabase();
        }

        public void Save(QuantityResponseDTO response)
        {
            var json = JsonSerializer.Serialize(response);
            _db.ListLeftPush(ListKey, json);
        }

        public IReadOnlyList<QuantityResponseDTO> GetAll()
        {
            var all = _db.ListRange(ListKey);
            return all.Select(v => JsonSerializer.Deserialize<QuantityResponseDTO>((string)v!)!)
                      .ToList()
                      .AsReadOnly();
        }

        public IReadOnlyList<QuantityResponseDTO> GetRecent(int count)
        {
            var items = _db.ListRange(ListKey, 0, count - 1);
            return items.Select(v => JsonSerializer.Deserialize<QuantityResponseDTO>((string)v!)!)
                        .ToList()
                        .AsReadOnly();
        }

        public IReadOnlyList<QuantityResponseDTO> GetByOperation(string operation)
        {
            return GetAll()
                .Where(r => r.Operation.Equals(operation, StringComparison.OrdinalIgnoreCase))
                .ToList()
                .AsReadOnly();
        }

        public IReadOnlyList<QuantityResponseDTO> GetByCategory(string category)
        {
            return GetAll()
                .Where(r =>
                    (r.Operand1?.Category ?? "").Equals(category, StringComparison.OrdinalIgnoreCase))
                .ToList()
                .AsReadOnly();
        }

        public int GetTotalCount() => (int)_db.ListLength(ListKey);

        public string GetPoolStatistics() => $"Redis - {GetTotalCount()} records";

        public void Clear() => _db.KeyDelete(ListKey);

        public void ReleaseResources() { }
    }
}
