using QuantityMeasurement.Model.Entities;
using QuantityMeasurement.Model.Units;
using QuantityMeasurement.BusinessLayer.Interfaces;
using QuantityMeasurement.Model.DTOs;
using QuantityMeasurement.Repository.Interfaces;

namespace QuantityMeasurement.BusinessLayer.Services
{
    public class QuantityService : IQuantityService
    {
        private readonly IQuantityMeasurementRepository repo;

        public QuantityService(IQuantityMeasurementRepository repo)
        {
            this.repo = repo;
        }

        // LENGTH 

        public QuantityResponseDTO AddLength(double v1, string u1, double v2, string u2)
        {
            try
            {
                var unit1 = Enum.Parse<LengthUnit>(u1);
                var unit2 = Enum.Parse<LengthUnit>(u2);

                var q1 = new Quantity<LengthUnit>(v1, unit1);
                var q2 = new Quantity<LengthUnit>(v2, unit2);

                var result = q1.Add(q2);

                var response = QuantityResponseDTO.ForArithmetic(
                    "Add",
                    new QuantityDTO(v1, u1, "Length"),
                    new QuantityDTO(v2, u2, "Length"),
                    new QuantityDTO(result.GetValue(), unit1.ToString(), "Length")
                );

                repo.Save(response);
                return response;
            }
            catch (Exception ex)
            {
                return QuantityResponseDTO.ForError("Add", ex.Message);
            }
        }

        public QuantityResponseDTO ConvertLength(double value, string fromUnit, string toUnit)
        {
            try
            {
                var from = Enum.Parse<LengthUnit>(fromUnit);
                var to = Enum.Parse<LengthUnit>(toUnit);

                var result = new Quantity<LengthUnit>(value, from).ConvertTo(to);

                var response = QuantityResponseDTO.ForConversion(
                    new QuantityDTO(value, fromUnit, "Length"),
                    new QuantityDTO(result.GetValue(), to.ToString(), "Length")
                );

                repo.Save(response);
                return response;
            }
            catch (Exception ex)
            {
                return QuantityResponseDTO.ForError("Convert", ex.Message);
            }
        }

        public QuantityResponseDTO CompareLength(double v1, string u1, double v2, string u2)
        {
            try
            {
                var q1 = new Quantity<LengthUnit>(v1, Enum.Parse<LengthUnit>(u1));
                var q2 = new Quantity<LengthUnit>(v2, Enum.Parse<LengthUnit>(u2));

                bool result = q1.Equals(q2);

                var response = QuantityResponseDTO.ForArithmetic(
                    "Compare",
                    new QuantityDTO(v1, u1, "Length"),
                    new QuantityDTO(v2, u2, "Length"),
                    new QuantityDTO(result ? 1 : 0, "Result", "Comparison")
                );

                repo.Save(response);
                return response;
            }
            catch (Exception ex)
            {
                return QuantityResponseDTO.ForError("Compare", ex.Message);
            }
        }

        public QuantityResponseDTO AddLengthWithTarget(double v1, string u1, double v2, string u2, string targetUnit)
        {
            try
            {
                var q1 = new Quantity<LengthUnit>(v1, Enum.Parse<LengthUnit>(u1));
                var q2 = new Quantity<LengthUnit>(v2, Enum.Parse<LengthUnit>(u2));
                var target = Enum.Parse<LengthUnit>(targetUnit);

                var result = q1.Add(q2, target);

                var response = QuantityResponseDTO.ForArithmetic(
                    "AddWithTarget",
                    new QuantityDTO(v1, u1, "Length"),
                    new QuantityDTO(v2, u2, "Length"),
                    new QuantityDTO(result.GetValue(), targetUnit, "Length")
                );

                repo.Save(response);
                return response;
            }
            catch (Exception ex)
            {
                return QuantityResponseDTO.ForError("AddWithTarget", ex.Message);
            }
        }

        public QuantityResponseDTO SubtractLength(double v1, string u1, double v2, string u2)
        {
            try
            {
                var q1 = new Quantity<LengthUnit>(v1, Enum.Parse<LengthUnit>(u1));
                var q2 = new Quantity<LengthUnit>(v2, Enum.Parse<LengthUnit>(u2));

                var result = q1.Subtract(q2);

                var response = QuantityResponseDTO.ForArithmetic(
                    "Subtract",
                    new QuantityDTO(v1, u1, "Length"),
                    new QuantityDTO(v2, u2, "Length"),
                    new QuantityDTO(result.GetValue(), u1, "Length")
                );

                repo.Save(response);
                return response;
            }
            catch (Exception ex)
            {
                return QuantityResponseDTO.ForError("Subtract", ex.Message);
            }
        }

        public QuantityResponseDTO DivideLength(double v1, string u1, double v2, string u2)
        {
            try
            {
                var q1 = new Quantity<LengthUnit>(v1, Enum.Parse<LengthUnit>(u1));
                var q2 = new Quantity<LengthUnit>(v2, Enum.Parse<LengthUnit>(u2));

                var result = q1.Divide(q2);

                var response = QuantityResponseDTO.ForArithmetic(
                    "Divide",
                    new QuantityDTO(v1, u1, "Length"),
                    new QuantityDTO(v2, u2, "Length"),
                    new QuantityDTO(result, "Scalar", "Division")
                );

                repo.Save(response);
                return response;
            }
            catch (Exception ex)
            {
                return QuantityResponseDTO.ForError("Divide", ex.Message);
            }
        }

        // ===== WEIGHT =====

        public QuantityResponseDTO AddWeight(double v1, string u1, double v2, string u2)
        {
            try
            {
                var result = new Quantity<WeightUnit>(v1, Enum.Parse<WeightUnit>(u1))
                    .Add(new Quantity<WeightUnit>(v2, Enum.Parse<WeightUnit>(u2)));

                var response = QuantityResponseDTO.ForArithmetic(
                    "AddWeight",
                    new QuantityDTO(v1, u1, "Weight"),
                    new QuantityDTO(v2, u2, "Weight"),
                    new QuantityDTO(result.GetValue(), u1, "Weight")
                );

                repo.Save(response);
                return response;
            }
            catch (Exception ex)
            {
                return QuantityResponseDTO.ForError("AddWeight", ex.Message);
            }
        }

        public QuantityResponseDTO SubtractWeight(double v1, string u1, double v2, string u2)
        {
            try
            {
                var q1 = new Quantity<WeightUnit>(v1, Enum.Parse<WeightUnit>(u1));
                var q2 = new Quantity<WeightUnit>(v2, Enum.Parse<WeightUnit>(u2));

                var result = q1.Subtract(q2);

                var response = QuantityResponseDTO.ForArithmetic(
                    "SubtractWeight",
                    new QuantityDTO(v1, u1, "Weight"),
                    new QuantityDTO(v2, u2, "Weight"),
                    new QuantityDTO(result.GetValue(), u1, "Weight")
                );

                repo.Save(response);
                return response;
            }
            catch (Exception ex)
            {
                return QuantityResponseDTO.ForError("SubtractWeight", ex.Message);
            }
        }

        public QuantityResponseDTO DivideWeight(double v1, string u1, double v2, string u2)
        {
            try
            {
                var q1 = new Quantity<WeightUnit>(v1, Enum.Parse<WeightUnit>(u1));
                var q2 = new Quantity<WeightUnit>(v2, Enum.Parse<WeightUnit>(u2));

                var result = q1.Divide(q2);

                var response = QuantityResponseDTO.ForArithmetic(
                    "DivideWeight",
                    new QuantityDTO(v1, u1, "Weight"),
                    new QuantityDTO(v2, u2, "Weight"),
                    new QuantityDTO(result, "Scalar", "Division")
                );

                repo.Save(response);
                return response;
            }
            catch (Exception ex)
            {
                return QuantityResponseDTO.ForError("DivideWeight", ex.Message);
            }
        }

        public QuantityResponseDTO ConvertWeight(double value, string fromUnit, string toUnit)
        {
            try
            {
                var result = new Quantity<WeightUnit>(value, Enum.Parse<WeightUnit>(fromUnit))
                    .ConvertTo(Enum.Parse<WeightUnit>(toUnit));

                var response = QuantityResponseDTO.ForConversion(
                    new QuantityDTO(value, fromUnit, "Weight"),
                    new QuantityDTO(result.GetValue(), toUnit, "Weight")
                );

                repo.Save(response);
                return response;
            }
            catch (Exception ex)
            {
                return QuantityResponseDTO.ForError("ConvertWeight", ex.Message);
            }
        }

        public QuantityResponseDTO CompareWeight(double v1, string u1, double v2, string u2)
        {
            try
            {
                bool result = new Quantity<WeightUnit>(v1, Enum.Parse<WeightUnit>(u1))
                    .Equals(new Quantity<WeightUnit>(v2, Enum.Parse<WeightUnit>(u2)));

                var response = QuantityResponseDTO.ForArithmetic(
                    "CompareWeight",
                    new QuantityDTO(v1, u1, "Weight"),
                    new QuantityDTO(v2, u2, "Weight"),
                    new QuantityDTO(result ? 1 : 0, "Boolean", "Comparison")
                );

                repo.Save(response);
                return response;
            }
            catch (Exception ex)
            {
                return QuantityResponseDTO.ForError("CompareWeight", ex.Message);
            }
        }

        // ===== VOLUME =====

        public QuantityResponseDTO AddVolume(double v1, string u1, double v2, string u2)
        {
            try
            {
                var result = new Quantity<VolumeUnit>(v1, Enum.Parse<VolumeUnit>(u1))
                    .Add(new Quantity<VolumeUnit>(v2, Enum.Parse<VolumeUnit>(u2)));

                var response = QuantityResponseDTO.ForArithmetic(
                    "AddVolume",
                    new QuantityDTO(v1, u1, "Volume"),
                    new QuantityDTO(v2, u2, "Volume"),
                    new QuantityDTO(result.GetValue(), u1, "Volume")
                );

                repo.Save(response);
                return response;
            }
            catch (Exception ex)
            {
                return QuantityResponseDTO.ForError("AddVolume", ex.Message);
            }
        }

        public QuantityResponseDTO SubtractVolume(double v1, string u1, double v2, string u2)
        {
            try
            {
                var q1 = new Quantity<VolumeUnit>(v1, Enum.Parse<VolumeUnit>(u1));
                var q2 = new Quantity<VolumeUnit>(v2, Enum.Parse<VolumeUnit>(u2));

                var result = q1.Subtract(q2);

                var response = QuantityResponseDTO.ForArithmetic(
                    "SubtractVolume",
                    new QuantityDTO(v1, u1, "Volume"),
                    new QuantityDTO(v2, u2, "Volume"),
                    new QuantityDTO(result.GetValue(), u1, "Volume")
                );

                repo.Save(response);
                return response;
            }
            catch (Exception ex)
            {
                return QuantityResponseDTO.ForError("SubtractVolume", ex.Message);
            }
        }

        public QuantityResponseDTO DivideVolume(double v1, string u1, double v2, string u2)
        {
            try
            {
                var q1 = new Quantity<VolumeUnit>(v1, Enum.Parse<VolumeUnit>(u1));
                var q2 = new Quantity<VolumeUnit>(v2, Enum.Parse<VolumeUnit>(u2));

                var result = q1.Divide(q2);

                var response = QuantityResponseDTO.ForArithmetic(
                    "DivideVolume",
                    new QuantityDTO(v1, u1, "Volume"),
                    new QuantityDTO(v2, u2, "Volume"),
                    new QuantityDTO(result, "Scalar", "Division")
                );

                repo.Save(response);
                return response;
            }
            catch (Exception ex)
            {
                return QuantityResponseDTO.ForError("DivideVolume", ex.Message);
            }
        }

        public QuantityResponseDTO ConvertVolume(double value, string fromUnit, string toUnit)
        {
            try
            {
                var result = new Quantity<VolumeUnit>(value, Enum.Parse<VolumeUnit>(fromUnit))
                    .ConvertTo(Enum.Parse<VolumeUnit>(toUnit));

                var response = QuantityResponseDTO.ForConversion(
                    new QuantityDTO(value, fromUnit, "Volume"),
                    new QuantityDTO(result.GetValue(), toUnit, "Volume")
                );

                repo.Save(response);
                return response;
            }
            catch (Exception ex)
            {
                return QuantityResponseDTO.ForError("ConvertVolume", ex.Message);
            }
        }

        public QuantityResponseDTO CompareVolume(double v1, string u1, double v2, string u2)
        {
            try
            {
                bool result = new Quantity<VolumeUnit>(v1, Enum.Parse<VolumeUnit>(u1))
                    .Equals(new Quantity<VolumeUnit>(v2, Enum.Parse<VolumeUnit>(u2)));

                var response = QuantityResponseDTO.ForArithmetic(
                    "CompareVolume",
                    new QuantityDTO(v1, u1, "Volume"),
                    new QuantityDTO(v2, u2, "Volume"),
                    new QuantityDTO(result ? 1 : 0, "Boolean", "Comparison")
                );

                repo.Save(response);
                return response;
            }
            catch (Exception ex)
            {
                return QuantityResponseDTO.ForError("CompareVolume", ex.Message);
            }
        }

        // ===== TEMPERATURE =====

        private TemperatureUnit ParseTemperatureUnit(string unit)
        {
            return unit switch
            {
                "Celsius" => TemperatureUnit.Celsius,
                "Fahrenheit" => TemperatureUnit.Fahrenheit,
                "Kelvin" => TemperatureUnit.Kelvin,
                _ => throw new ArgumentException("Invalid temperature unit")
            };
        }

        public QuantityResponseDTO ConvertTemperature(double value, string fromUnit, string toUnit)
        {
            try
            {
                var result = new Quantity<TemperatureUnit>(value, ParseTemperatureUnit(fromUnit))
                    .ConvertTo(ParseTemperatureUnit(toUnit));

                var response = QuantityResponseDTO.ForConversion(
                    new QuantityDTO(value, fromUnit, "Temperature"),
                    new QuantityDTO(result.GetValue(), toUnit, "Temperature")
                );

                repo.Save(response);
                return response;
            }
            catch (Exception ex)
            {
                return QuantityResponseDTO.ForError("ConvertTemperature", ex.Message);
            }
        }

        public QuantityResponseDTO CompareTemperature(double v1, string u1, double v2, string u2)
        {
            try
            {
                bool result = new Quantity<TemperatureUnit>(v1, ParseTemperatureUnit(u1))
                    .Equals(new Quantity<TemperatureUnit>(v2, ParseTemperatureUnit(u2)));

                var response = QuantityResponseDTO.ForArithmetic(
                    "CompareTemperature",
                    new QuantityDTO(v1, u1, "Temperature"),
                    new QuantityDTO(v2, u2, "Temperature"),
                    new QuantityDTO(result ? 1 : 0, "Boolean", "Comparison")
                );

                repo.Save(response);
                return response;
            }
            catch (Exception ex)
            {
                return QuantityResponseDTO.ForError("CompareTemperature", ex.Message);
            }
        }

        // History methods - service owns these so the API layer never touches the repo directly

        public IReadOnlyList<QuantityResponseDTO> GetHistory()
        {
            return repo.GetAll();
        }

        public IReadOnlyList<QuantityResponseDTO> GetRecentHistory(int count)
        {
            return repo.GetRecent(count);
        }

        public IReadOnlyList<QuantityResponseDTO> GetHistoryByOperation(string operation)
        {
            return repo.GetByOperation(operation);
        }

        public IReadOnlyList<QuantityResponseDTO> GetHistoryByCategory(string category)
        {
            return repo.GetByCategory(category);
        }

        public int GetTotalCount()
        {
            return repo.GetTotalCount();
        }

        public Dictionary<string, int> GetOperationStats()
        {
            var all = repo.GetAll();
            return all
                .GroupBy(r => r.Operation)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public void ClearHistory()
        {
            repo.Clear();
        }
    }
}
