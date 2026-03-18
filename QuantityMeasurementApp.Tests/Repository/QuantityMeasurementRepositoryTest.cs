using QuantityMeasurement.Repository;
using QuantityMeasurement.Repository.Interfaces;
using QuantityMeasurement.Model.DTOs;

namespace QuantityMeasurementAppTest.Repository
{
    [TestFixture]
    public class QuantityMeasurementRepositoryTest
    {
        private IQuantityMeasurementRepository _repo = null!;

        // sample DTOs reused across tests
        private static QuantityResponseDTO MakeAddLength(double r = 5.0) =>
            QuantityResponseDTO.ForArithmetic(
                "Add",
                new QuantityDTO(3.0, "Feet", "Length"),
                new QuantityDTO(2.0, "Feet", "Length"),
                new QuantityDTO(r,   "Feet", "Length")
            );

        private static QuantityResponseDTO MakeConvertWeight() =>
            QuantityResponseDTO.ForConversion(
                new QuantityDTO(1.0,    "Kilogram", "Weight"),
                new QuantityDTO(1000.0, "Gram",     "Weight")
            );

        private static QuantityResponseDTO MakeCompareVolume(bool equal = true) =>
            QuantityResponseDTO.ForArithmetic(
                "Compare",
                new QuantityDTO(1.0,    "Litre",      "Volume"),
                new QuantityDTO(1000.0, "Millilitre", "Volume"),
                new QuantityDTO(equal ? 1 : 0, "Result", "Comparison")
            );

        private static QuantityResponseDTO MakeError() =>
            QuantityResponseDTO.ForError("Add", "Test error message");

        [SetUp]
        public void Setup()
        {
            _repo = new QuantityMeasurementCacheRepository();
        }

        // Save & GetAll

        [Test]
        public void Save_SingleRecord_GetAllReturnsOne()
        {
            _repo.Save(MakeAddLength());
            Assert.That(_repo.GetAll().Count, Is.EqualTo(1));
        }

        [Test]
        public void Save_MultipleRecords_GetAllReturnsAll()
        {
            _repo.Save(MakeAddLength());
            _repo.Save(MakeConvertWeight());
            _repo.Save(MakeCompareVolume());
            Assert.That(_repo.GetAll().Count, Is.EqualTo(3));
        }

        [Test]
        public void Save_ErrorResponse_StoredCorrectly()
        {
            _repo.Save(MakeError());
            var all = _repo.GetAll();
            Assert.That(all.Count,          Is.EqualTo(1));
            Assert.That(all[0].Success,     Is.False);
            Assert.That(all[0].ErrorMessage, Is.EqualTo("Test error message"));
        }

        [Test]
        public void GetAll_EmptyRepo_ReturnsEmptyList()
        {
            Assert.That(_repo.GetAll(), Is.Empty);
        }

        [Test]
        public void GetAll_ReturnsReadOnlyList()
        {
            _repo.Save(MakeAddLength());
            IReadOnlyList<QuantityResponseDTO> all = _repo.GetAll();
            Assert.That(all, Is.Not.Null);
        }

        // Clear

        [Test]
        public void Clear_AfterSave_CountBecomesZero()
        {
            _repo.Save(MakeAddLength());
            _repo.Save(MakeConvertWeight());
            _repo.Clear();
            Assert.That(_repo.GetTotalCount(), Is.EqualTo(0));
        }

        [Test]
        public void Clear_OnEmptyRepo_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => _repo.Clear());
        }

        [Test]
        public void Clear_ThenSave_Works()
        {
            _repo.Save(MakeAddLength());
            _repo.Clear();
            _repo.Save(MakeConvertWeight());
            Assert.That(_repo.GetTotalCount(), Is.EqualTo(1));
        }

        // GetTotalCount

        [Test]
        public void GetTotalCount_EmptyRepo_ReturnsZero()
        {
            Assert.That(_repo.GetTotalCount(), Is.EqualTo(0));
        }

        [Test]
        public void GetTotalCount_AfterThreeSaves_ReturnsThree()
        {
            _repo.Save(MakeAddLength());
            _repo.Save(MakeConvertWeight());
            _repo.Save(MakeCompareVolume());
            Assert.That(_repo.GetTotalCount(), Is.EqualTo(3));
        }

        [Test]
        public void GetTotalCount_MatchesGetAllCount()
        {
            _repo.Save(MakeAddLength());
            _repo.Save(MakeConvertWeight());
            Assert.That(_repo.GetTotalCount(), Is.EqualTo(_repo.GetAll().Count));
        }

        // GetByOperation

        [Test]
        public void GetByOperation_Add_ReturnsOnlyAddRecords()
        {
            _repo.Save(MakeAddLength());
            _repo.Save(MakeConvertWeight());
            _repo.Save(MakeCompareVolume());

            var result = _repo.GetByOperation("Add");
            Assert.That(result.Count,          Is.EqualTo(1));
            Assert.That(result[0].Operation,   Is.EqualTo("Add"));
        }

        [Test]
        public void GetByOperation_CaseInsensitive_ReturnsMatch()
        {
            _repo.Save(MakeConvertWeight());
            var result = _repo.GetByOperation("convert");
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetByOperation_NoMatch_ReturnsEmpty()
        {
            _repo.Save(MakeAddLength());
            Assert.That(_repo.GetByOperation("Subtract"), Is.Empty);
        }

        [Test]
        public void GetByOperation_MultipleMatchingSameOp_ReturnsAll()
        {
            _repo.Save(MakeAddLength(3.0));
            _repo.Save(MakeAddLength(7.0));
            _repo.Save(MakeConvertWeight());

            Assert.That(_repo.GetByOperation("Add").Count, Is.EqualTo(2));
        }

        // GetByCategory

        [Test]
        public void GetByCategory_Length_ReturnsOnlyLengthRecords()
        {
            _repo.Save(MakeAddLength());
            _repo.Save(MakeConvertWeight());
            _repo.Save(MakeCompareVolume());

            var result = _repo.GetByCategory("Length");
            Assert.That(result.Count,                      Is.EqualTo(1));
            Assert.That(result[0].Operand1!.Category,      Is.EqualTo("Length"));
        }

        [Test]
        public void GetByCategory_CaseInsensitive_ReturnsMatch()
        {
            _repo.Save(MakeConvertWeight());
            Assert.That(_repo.GetByCategory("weight").Count, Is.EqualTo(1));
        }

        [Test]
        public void GetByCategory_NoMatch_ReturnsEmpty()
        {
            _repo.Save(MakeAddLength());
            Assert.That(_repo.GetByCategory("Temperature"), Is.Empty);
        }

        [Test]
        public void GetByCategory_Volume_ReturnsBothVolumeRecords()
        {
            _repo.Save(MakeCompareVolume(true));
            _repo.Save(MakeCompareVolume(false));
            _repo.Save(MakeAddLength());

            Assert.That(_repo.GetByCategory("Volume").Count, Is.EqualTo(2));
        }

        // default interface methods

        [Test]
        public void GetPoolStatistics_CacheRepo_ReturnsNAString()
        {
            Assert.That(_repo.GetPoolStatistics(), Does.Contain("N/A"));
        }

        [Test]
        public void ReleaseResources_CacheRepo_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => _repo.ReleaseResources());
        }

        // data integrity

        [Test]
        public void Save_PreservesOperand1Fields()
        {
            var dto = QuantityResponseDTO.ForArithmetic(
                "Add",
                new QuantityDTO(10.5, "Inches", "Length"),
                new QuantityDTO(2.0,  "Feet",   "Length"),
                new QuantityDTO(0.875,"Feet",   "Length")
            );
            _repo.Save(dto);

            var stored = _repo.GetAll()[0];
            Assert.That(stored.Operand1!.Value,   Is.EqualTo(10.5));
            Assert.That(stored.Operand1.Unit,     Is.EqualTo("Inches"));
            Assert.That(stored.Operand1.Category, Is.EqualTo("Length"));
        }

        [Test]
        public void Save_ConversionRecord_Operand2IsNull()
        {
            _repo.Save(MakeConvertWeight());
            Assert.That(_repo.GetAll()[0].Operand2, Is.Null);
        }

        [Test]
        public void Save_PreservesResultFields()
        {
            _repo.Save(MakeConvertWeight());

            var stored = _repo.GetAll()[0];
            Assert.That(stored.Result!.Value,   Is.EqualTo(1000.0));
            Assert.That(stored.Result.Unit,     Is.EqualTo("Gram"));
            Assert.That(stored.Result.Category, Is.EqualTo("Weight"));
        }
    }
}
