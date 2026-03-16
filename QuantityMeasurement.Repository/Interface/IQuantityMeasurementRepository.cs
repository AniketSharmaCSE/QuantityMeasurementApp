using QuantityMeasurement.Model.DTOs;

namespace QuantityMeasurement.Repository.Interfaces
{
    public interface IQuantityMeasurementRepository
    {
        void Save(QuantityResponseDTO response);

        IReadOnlyList<QuantityResponseDTO> GetAll();

        void Clear();
    }
}