using NUnit.Framework;
using QuantityMeasurement.Model.DTOs;
using QuantityMeasurement.Repository;
using QuantityMeasurement.BusinessLayer.Services;

namespace QuantityMeasurementAppTest.Service
{
    [TestFixture]
    public class QuantityServiceTest
    {
        private QuantityMeasurementCacheRepository _repo = null!;
        private QuantityService _service = null!;

        [SetUp]
        public void SetUp()
        {
            _repo    = new QuantityMeasurementCacheRepository();
            _service = new QuantityService(_repo);
        }

        // Length

        [Test]
        public void AddLength_ValidInputs_ReturnsSuccessAndPersists()
        {
            var result = _service.AddLength(1.0, "Feet", 12.0, "Inches");

            Assert.That(result.Success,        Is.True);
            Assert.That(result.Operation,      Is.EqualTo("Add"));
            Assert.That(result.Result!.Value,  Is.EqualTo(2.0).Within(1e-6));
            Assert.That(_repo.GetTotalCount(), Is.EqualTo(1));
        }

        [Test]
        public void ConvertLength_FeetToInches_ReturnsCorrectValue()
        {
            var result = _service.ConvertLength(1.0, "Feet", "Inches");

            Assert.That(result.Success,       Is.True);
            Assert.That(result.Result!.Value, Is.EqualTo(12.0).Within(1e-6));
        }

        [Test]
        public void CompareLength_EqualLengths_ReturnsTrueResult()
        {
            var result = _service.CompareLength(1.0, "Yards", 3.0, "Feet");

            Assert.That(result.Success,       Is.True);
            Assert.That(result.Result!.Value, Is.EqualTo(1.0));
        }

        [Test]
        public void SubtractLength_CrossUnit_ReturnsCorrectValue()
        {
            var result = _service.SubtractLength(10.0, "Feet", 6.0, "Inches");

            Assert.That(result.Success,       Is.True);
            Assert.That(result.Result!.Value, Is.EqualTo(9.5).Within(1e-4));
        }

        [Test]
        public void DivideLength_SameUnit_ReturnsScalar()
        {
            var result = _service.DivideLength(10.0, "Feet", 2.0, "Feet");

            Assert.That(result.Success,       Is.True);
            Assert.That(result.Result!.Value, Is.EqualTo(5.0).Within(1e-6));
        }

        [Test]
        public void AddLengthWithTarget_Persists()
        {
            var result = _service.AddLengthWithTarget(1.0, "Yards", 36.0, "Inches", "Yards");

            Assert.That(result.Success,        Is.True);
            Assert.That(_repo.GetTotalCount(), Is.EqualTo(1));
        }

        // Weight

        [Test]
        public void AddWeight_KgPlusGram_ReturnsCorrectKgValue()
        {
            var result = _service.AddWeight(1.0, "Kilogram", 1000.0, "Gram");

            Assert.That(result.Success,       Is.True);
            Assert.That(result.Result!.Value, Is.EqualTo(2.0).Within(1e-4));
        }

        [Test]
        public void ConvertWeight_KilogramToGram_Returns1000()
        {
            var result = _service.ConvertWeight(1.0, "Kilogram", "Gram");

            Assert.That(result.Success,       Is.True);
            Assert.That(result.Result!.Value, Is.EqualTo(1000.0).Within(1e-4));
        }

        [Test]
        public void CompareWeight_EqualWeights_ReturnsTrueResult()
        {
            var result = _service.CompareWeight(1.0, "Kilogram", 1000.0, "Gram");

            Assert.That(result.Success,       Is.True);
            Assert.That(result.Result!.Value, Is.EqualTo(1.0));
        }

        // Volume

        [Test]
        public void AddVolume_LitrePlusMillilitre_ReturnsCorrectLitres()
        {
            var result = _service.AddVolume(1.0, "Litre", 1000.0, "Millilitre");

            Assert.That(result.Success,       Is.True);
            Assert.That(result.Result!.Value, Is.EqualTo(2.0).Within(1e-4));
        }

        [Test]
        public void ConvertVolume_LitreToMillilitre_Returns1000()
        {
            var result = _service.ConvertVolume(1.0, "Litre", "Millilitre");

            Assert.That(result.Success,       Is.True);
            Assert.That(result.Result!.Value, Is.EqualTo(1000.0).Within(1e-4));
        }

        // Temperature

        [Test]
        public void ConvertTemperature_CelsiusToFahrenheit_Returns212()
        {
            var result = _service.ConvertTemperature(100.0, "Celsius", "Fahrenheit");

            Assert.That(result.Success,       Is.True);
            Assert.That(result.Result!.Value, Is.EqualTo(212.0).Within(0.01));
        }

        [Test]
        public void CompareTemperature_ZeroCelsiusAnd32F_ReturnsTrueResult()
        {
            var result = _service.CompareTemperature(0.0, "Celsius", 32.0, "Fahrenheit");

            Assert.That(result.Success,       Is.True);
            Assert.That(result.Result!.Value, Is.EqualTo(1.0));
        }

        // Error handling

        [Test]
        public void AddLength_InvalidUnit_ReturnsErrorResponse()
        {
            var result = _service.AddLength(1.0, "INVALID_UNIT", 2.0, "Feet");

            Assert.That(result.Success,      Is.False);
            Assert.That(result.ErrorMessage, Is.Not.Empty);
        }

        [Test]
        public void DivideLength_ByZero_ReturnsErrorResponse()
        {
            var result = _service.DivideLength(10.0, "Feet", 0.0, "Feet");

            Assert.That(result.Success,      Is.False);
            Assert.That(result.ErrorMessage, Is.Not.Empty);
        }

        [Test]
        public void ErrorResponse_IsNotPersisted_CountRemainsZero()
        {
            _service.AddLength(1.0, "INVALID", 2.0, "Feet");
            Assert.That(_repo.GetTotalCount(), Is.EqualTo(0));
        }

        // persistence queries

        [Test]
        public void MultipleOperations_AllPersisted_GetTotalCountCorrect()
        {
            _service.AddLength(1.0, "Feet", 2.0, "Feet");
            _service.ConvertWeight(1.0, "Kilogram", "Gram");
            _service.ConvertTemperature(100.0, "Celsius", "Fahrenheit");

            Assert.That(_repo.GetTotalCount(), Is.EqualTo(3));
        }

        [Test]
        public void OperationsOfSameType_QueryableByOperation()
        {
            // AddWeight uses operation name "AddWeight", not "Add"
            // so use two AddLength calls to get two "Add" records
            _service.AddLength(1.0, "Feet", 2.0, "Feet");
            _service.AddLength(3.0, "Feet", 4.0, "Feet");
            _service.ConvertLength(12.0, "Inches", "Feet");

            Assert.That(_repo.GetByOperation("Add").Count, Is.EqualTo(2));
        }

        [Test]
        public void LengthOperations_QueryableByCategory()
        {
            _service.AddLength(1.0, "Feet", 2.0, "Feet");
            _service.ConvertLength(12.0, "Inches", "Feet");
            _service.ConvertWeight(1.0, "Kilogram", "Gram");

            Assert.That(_repo.GetByCategory("Length").Count, Is.EqualTo(2));
        }
    }
}