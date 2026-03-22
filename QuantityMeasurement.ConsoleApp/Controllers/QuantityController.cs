using QuantityMeasurement.BusinessLayer.Interfaces;
using QuantityMeasurement.Model.DTOs;

namespace QuantityMeasurement.ConsoleApp.Controllers
{
    public class QuantityController
    {
        private readonly IQuantityService service;

        public QuantityController(IQuantityService service)
        {
            this.service = service;
        }

        public QuantityResponseDTO AddLength(double v1, string u1, double v2, string u2)
        {
            return service.AddLength(v1, u1, v2, u2);
        }

        public QuantityResponseDTO ConvertLength(double value, string fromUnit, string toUnit)
        {
            return service.ConvertLength(value, fromUnit, toUnit);
        }         
        public QuantityResponseDTO CompareLength(double v1, string u1, double v2, string u2)
        {
            return service.CompareLength(v1, u1, v2, u2);
        }

        public QuantityResponseDTO SubtractLength(double v1, string u1, double v2, string u2)
        {
            return service.SubtractLength(v1, u1, v2, u2);
        }

        public QuantityResponseDTO DivideLength(double v1, string u1, double v2, string u2)
        {
            return service.DivideLength(v1, u1, v2, u2);
        }

        public QuantityResponseDTO AddWeight(double v1, string u1, double v2, string u2)
        {
            return service.AddWeight(v1, u1, v2, u2);
        }

        public QuantityResponseDTO SubtractWeight(double v1, string u1, double v2, string u2)
        {
            return service.SubtractWeight(v1, u1, v2, u2);
        }

        public QuantityResponseDTO DivideWeight(double v1, string u1, double v2, string u2)
        {
            return service.DivideWeight(v1, u1, v2, u2);
        }

        public QuantityResponseDTO ConvertWeight(double value, string fromUnit, string toUnit)
        {
            return service.ConvertWeight(value, fromUnit, toUnit);
        }

        public QuantityResponseDTO CompareWeight(double v1, string u1, double v2, string u2)
        {
            return service.CompareWeight(v1, u1, v2, u2);
        }
        public QuantityResponseDTO AddLengthWithTarget(double v1, string u1, double v2, string u2, string targetUnit)
        {
            return service.AddLengthWithTarget(v1, u1, v2, u2, targetUnit);
        }

        public QuantityResponseDTO AddVolume(double v1, string u1, double v2, string u2)
        {
            return service.AddVolume(v1, u1, v2, u2);
        }

        public QuantityResponseDTO SubtractVolume(double v1, string u1, double v2, string u2)
        {
            return service.SubtractVolume(v1, u1, v2, u2);
        }

        public QuantityResponseDTO DivideVolume(double v1, string u1, double v2, string u2)
        {
            return service.DivideVolume(v1, u1, v2, u2);
        }

        public QuantityResponseDTO ConvertVolume(double value, string fromUnit, string toUnit)
        {
            return service.ConvertVolume(value, fromUnit, toUnit);
        }

        public QuantityResponseDTO CompareVolume(double v1, string u1, double v2, string u2)
        {
            return service.CompareVolume(v1, u1, v2, u2);
        }

        public QuantityResponseDTO ConvertTemperature(double value, string fromUnit, string toUnit)
        {
            return service.ConvertTemperature(value, fromUnit, toUnit);
        }

        public QuantityResponseDTO CompareTemperature(double v1, string u1, double v2, string u2)
        {
            return service.CompareTemperature(v1, u1, v2, u2);
        }

    }
}