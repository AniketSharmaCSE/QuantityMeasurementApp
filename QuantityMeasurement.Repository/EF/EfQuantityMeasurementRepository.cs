using QuantityMeasurement.Model.DTOs;
using QuantityMeasurement.Model.Entities.EF;
using QuantityMeasurement.Repository.Data;
using QuantityMeasurement.Repository.Interfaces;

namespace QuantityMeasurement.Repository.EF
{
    public class EfQuantityMeasurementRepository : IQuantityMeasurementRepository
    {
        private readonly AppDbContext _db;

        public EfQuantityMeasurementRepository(AppDbContext db)
        {
            _db = db;
        }

        public void Save(QuantityResponseDTO response)
        {
            var record = ToRecord(response);
            _db.Measurements.Add(record);
            _db.SaveChanges();
        }

        public IReadOnlyList<QuantityResponseDTO> GetAll()
        {
            return _db.Measurements
                .OrderByDescending(m => m.Timestamp)
                .ToList()
                .Select(ToDto)
                .ToList()
                .AsReadOnly();
        }

        public IReadOnlyList<QuantityResponseDTO> GetRecent(int count)
        {
            return _db.Measurements
                .OrderByDescending(m => m.Timestamp)
                .Take(count)
                .ToList()
                .Select(ToDto)
                .ToList()
                .AsReadOnly();
        }

        public IReadOnlyList<QuantityResponseDTO> GetByOperation(string operation)
        {
            return _db.Measurements
                .Where(m => m.Operation.ToLower() == operation.ToLower())
                .OrderByDescending(m => m.Timestamp)
                .ToList()
                .Select(ToDto)
                .ToList()
                .AsReadOnly();
        }

        public IReadOnlyList<QuantityResponseDTO> GetByCategory(string category)
        {
            return _db.Measurements
                .Where(m => m.Operand1Category != null &&
                            m.Operand1Category.ToLower() == category.ToLower())
                .OrderByDescending(m => m.Timestamp)
                .ToList()
                .Select(ToDto)
                .ToList()
                .AsReadOnly();
        }

        public int GetTotalCount() => _db.Measurements.Count();

        public string GetPoolStatistics() => $"EF/SQL Server - {_db.Measurements.Count()} records";

        public void Clear()
        {
            _db.Measurements.RemoveRange(_db.Measurements);
            _db.SaveChanges();
        }

        public void ReleaseResources() { }

        private static MeasurementRecord ToRecord(QuantityResponseDTO dto)
        {
            return new MeasurementRecord
            {
                Operation        = dto.Operation,
                Operand1Value    = dto.Operand1?.Value,
                Operand1Unit     = dto.Operand1?.Unit,
                Operand1Category = dto.Operand1?.Category,
                Operand2Value    = dto.Operand2?.Value,
                Operand2Unit     = dto.Operand2?.Unit,
                Operand2Category = dto.Operand2?.Category,
                ResultValue      = dto.Result?.Value,
                ResultUnit       = dto.Result?.Unit,
                ResultCategory   = dto.Result?.Category,
                BoolResult       = dto.BoolResult,
                ScalarResult     = dto.ScalarResult,
                HasError         = !dto.Success,
                ErrorMessage     = dto.ErrorMessage,
                Timestamp        = dto.Timestamp
            };
        }

        private static QuantityResponseDTO ToDto(MeasurementRecord r)
        {
            return new QuantityResponseDTO
            {
                Success      = !r.HasError,
                Operation    = r.Operation,
                Operand1     = r.Operand1Value.HasValue
                    ? new QuantityDTO(r.Operand1Value.Value, r.Operand1Unit ?? "", r.Operand1Category ?? "")
                    : null,
                Operand2     = r.Operand2Value.HasValue
                    ? new QuantityDTO(r.Operand2Value.Value, r.Operand2Unit ?? "", r.Operand2Category ?? "")
                    : null,
                Result       = r.ResultValue.HasValue
                    ? new QuantityDTO(r.ResultValue.Value, r.ResultUnit ?? "", r.ResultCategory ?? "")
                    : null,
                BoolResult   = r.BoolResult,
                ScalarResult = r.ScalarResult,
                ErrorMessage = r.ErrorMessage,
                Timestamp    = r.Timestamp
            };
        }
    }
}
