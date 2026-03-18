using NUnit.Framework;
using QuantityMeasurement.BusinessLayer.Controllers;
using QuantityMeasurement.BusinessLayer.Interfaces;
using QuantityMeasurement.BusinessLayer.Services;
using QuantityMeasurement.Repository;
using QuantityMeasurement.Repository.Interfaces;

namespace QuantityMeasurementAppTest.Integration
{
    // full stack tests with no mocks – wires up Controller → Service → Repository using real objects
    [TestFixture]
    public class QuantityMeasurementIntegrationTest
    {
        private IQuantityMeasurementRepository _repo = null!;
        private IQuantityService               _service = null!;
        private QuantityController             _controller = null!;

        [SetUp]
        public void Setup()
        {
            _repo       = new QuantityMeasurementCacheRepository();
            _service    = new QuantityService(_repo);
            _controller = new QuantityController(_service);
        }

        [TearDown]
        public void TearDown()
        {
            _repo.Clear();
            _repo.ReleaseResources();
        }

        // persistence

        [Test]
        public void AddLength_SavedToRepository()
        {
            _controller.AddLength(1.0, "Feet", 2.0, "Feet");
            Assert.That(_repo.GetTotalCount(), Is.EqualTo(1));
        }

        [Test]
        public void ConvertWeight_SavedToRepository()
        {
            _controller.ConvertWeight(1.0, "Kilogram", "Gram");
            Assert.That(_repo.GetTotalCount(), Is.EqualTo(1));
        }

        [Test]
        public void MultipleOperations_AllSavedToRepository()
        {
            _controller.AddLength(1.0, "Feet", 2.0, "Feet");
            _controller.ConvertWeight(1.0, "Kilogram", "Gram");
            _controller.CompareVolume(1.0, "Litre", 1000.0, "Millilitre");
            _controller.ConvertTemperature(0.0, "Celsius", "Fahrenheit");

            Assert.That(_repo.GetTotalCount(), Is.EqualTo(4));
        }

        [Test]
        public void ErrorOperation_NotSavedToRepository()
        {
            _controller.AddLength(1.0, "BADUNIT", 2.0, "Feet");
            Assert.That(_repo.GetTotalCount(), Is.EqualTo(0));
        }

        // GetAll correctness

        [Test]
        public void GetAll_AfterAddLength_ContainsCorrectOperation()
        {
            _controller.AddLength(3.0, "Feet", 9.0, "Inches");

            var all = _repo.GetAll();
            Assert.That(all.Count,        Is.EqualTo(1));
            Assert.That(all[0].Operation, Is.EqualTo("Add"));
            Assert.That(all[0].Success,   Is.True);
        }

        [Test]
        public void GetAll_AfterConvert_OperandAndResultPreserved()
        {
            _controller.ConvertLength(1.0, "Feet", "Inches");

            var record = _repo.GetAll()[0];
            Assert.That(record.Operation,       Is.EqualTo("Convert"));
            Assert.That(record.Operand1!.Value, Is.EqualTo(1.0));
            Assert.That(record.Operand1.Unit,   Is.EqualTo("Feet"));
            Assert.That(record.Result!.Value,   Is.EqualTo(12.0).Within(1e-4));
            Assert.That(record.Result.Unit,     Is.EqualTo("Inches"));
        }

        // GetByOperation

        [Test]
        public void GetByOperation_AfterMixedOperations_FiltersCorrectly()
        {
            _controller.AddLength(1.0, "Feet", 2.0, "Feet");
            _controller.ConvertWeight(1.0, "Kilogram", "Gram");
            _controller.SubtractLength(10.0, "Feet", 3.0, "Feet");
            _controller.AddVolume(1.0, "Litre", 1.0, "Litre"); // "AddVolume" != "Add"

            Assert.That(_repo.GetByOperation("Add").Count, Is.EqualTo(1));
        }

        [Test]
        public void GetByOperation_Convert_ReturnsAllConversions()
        {
            _controller.ConvertLength(1.0, "Feet", "Inches");
            _controller.ConvertWeight(1.0, "Kilogram", "Gram");
            _controller.ConvertVolume(1.0, "Litre", "Millilitre");
            _controller.AddLength(1.0, "Feet", 2.0, "Feet");

            Assert.That(_repo.GetByOperation("Convert").Count, Is.EqualTo(3));
        }

        // GetByCategory

        [Test]
        public void GetByCategory_Length_ReturnsOnlyLengthOps()
        {
            _controller.AddLength(1.0, "Feet", 2.0, "Feet");
            _controller.ConvertWeight(1.0, "Kilogram", "Gram");
            _controller.CompareVolume(1.0, "Litre", 1.0, "Litre");

            var lengthOps = _repo.GetByCategory("Length");
            Assert.That(lengthOps.Count,                    Is.EqualTo(1));
            Assert.That(lengthOps[0].Operand1!.Category,   Is.EqualTo("Length"));
        }

        [Test]
        public void GetByCategory_Weight_ReturnsAllWeightOps()
        {
            _controller.AddWeight(1.0, "Kilogram", 500.0, "Gram");
            _controller.ConvertWeight(2.0, "Kilogram", "Gram");
            _controller.CompareWeight(1.0, "Kilogram", 1000.0, "Gram");
            _controller.AddLength(1.0, "Feet", 1.0, "Feet"); // should not be included

            Assert.That(_repo.GetByCategory("Weight").Count, Is.EqualTo(3));
        }

        // GetTotalCount

        [Test]
        public void GetTotalCount_IncreasesWithEachOperation()
        {
            Assert.That(_repo.GetTotalCount(), Is.EqualTo(0));

            _controller.AddLength(1.0, "Feet", 1.0, "Feet");
            Assert.That(_repo.GetTotalCount(), Is.EqualTo(1));

            _controller.ConvertWeight(1.0, "Kilogram", "Gram");
            Assert.That(_repo.GetTotalCount(), Is.EqualTo(2));
        }

        // Clear

        [Test]
        public void Clear_AfterOperations_RepoIsEmpty()
        {
            _controller.AddLength(1.0, "Feet", 2.0, "Feet");
            _controller.ConvertWeight(1.0, "Kilogram", "Gram");

            _repo.Clear();

            Assert.That(_repo.GetTotalCount(), Is.EqualTo(0));
            Assert.That(_repo.GetAll(),        Is.Empty);
        }

        // numeric correctness

        [Test]
        public void AddLength_FeetPlusFeet_ResultIsCorrect()
        {
            var response = _controller.AddLength(3.0, "Feet", 2.0, "Feet");

            Assert.That(response.Success,       Is.True);
            Assert.That(response.Result!.Value, Is.EqualTo(5.0));
        }

        [Test]
        public void ConvertTemperature_BoilingPoint_CelsiusToFahrenheit()
        {
            var response = _controller.ConvertTemperature(100.0, "Celsius", "Fahrenheit");

            Assert.That(response.Success,       Is.True);
            Assert.That(response.Result!.Value, Is.EqualTo(212.0).Within(0.01));
        }

        [Test]
        public void CompareLength_OneYardEqualsThreeFeet_ReturnsEqual()
        {
            var response = _controller.CompareLength(1.0, "Yards", 3.0, "Feet");

            Assert.That(response.Success,       Is.True);
            Assert.That(response.Result!.Value, Is.EqualTo(1.0)); // 1 = equal
        }

        [Test]
        public void GetPoolStatistics_CacheRepo_ReturnsNAString()
        {
            Assert.That(_repo.GetPoolStatistics(), Does.Contain("N/A"));
        }
    }
}
