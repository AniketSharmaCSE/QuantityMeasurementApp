using Moq;
using QuantityMeasurement.BusinessLayer.Controllers;
using QuantityMeasurement.BusinessLayer.Interfaces;
using QuantityMeasurement.Model.DTOs;

namespace QuantityMeasurementAppTest.Controller
{
    [TestFixture]
    public class QuantityControllerTest
    {
        private Mock<IQuantityService> _mockService = null!;
        private QuantityController _controller = null!;

        private static readonly QuantityResponseDTO SuccessResponse =
            QuantityResponseDTO.ForArithmetic(
                "Add",
                new QuantityDTO(1.0, "Feet", "Length"),
                new QuantityDTO(2.0, "Feet", "Length"),
                new QuantityDTO(3.0, "Feet", "Length")
            );

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IQuantityService>();
            _controller  = new QuantityController(_mockService.Object);
        }

        // Length

        [Test]
        public void AddLength_DelegatesToService_ReturnsServiceResult()
        {
            _mockService.Setup(s => s.AddLength(1.0, "Feet", 2.0, "Feet")).Returns(SuccessResponse);

            var result = _controller.AddLength(1.0, "Feet", 2.0, "Feet");

            Assert.That(result, Is.SameAs(SuccessResponse));
            _mockService.Verify(s => s.AddLength(1.0, "Feet", 2.0, "Feet"), Times.Once);
        }

        [Test]
        public void ConvertLength_DelegatesToService_ReturnsServiceResult()
        {
            var expected = QuantityResponseDTO.ForConversion(
                new QuantityDTO(1.0,  "Feet",   "Length"),
                new QuantityDTO(12.0, "Inches", "Length")
            );
            _mockService.Setup(s => s.ConvertLength(1.0, "Feet", "Inches")).Returns(expected);

            var result = _controller.ConvertLength(1.0, "Feet", "Inches");

            Assert.That(result, Is.SameAs(expected));
        }

        [Test]
        public void CompareLength_DelegatesToService()
        {
            _mockService.Setup(s => s.CompareLength(It.IsAny<double>(), It.IsAny<string>(),
                                                    It.IsAny<double>(), It.IsAny<string>()))
                        .Returns(SuccessResponse);

            _controller.CompareLength(1.0, "Yards", 3.0, "Feet");

            _mockService.Verify(s => s.CompareLength(1.0, "Yards", 3.0, "Feet"), Times.Once);
        }

        [Test]
        public void SubtractLength_DelegatesToService()
        {
            _mockService.Setup(s => s.SubtractLength(It.IsAny<double>(), It.IsAny<string>(),
                                                     It.IsAny<double>(), It.IsAny<string>()))
                        .Returns(SuccessResponse);

            _controller.SubtractLength(10.0, "Feet", 2.0, "Feet");

            _mockService.Verify(s => s.SubtractLength(10.0, "Feet", 2.0, "Feet"), Times.Once);
        }

        [Test]
        public void DivideLength_DelegatesToService()
        {
            _mockService.Setup(s => s.DivideLength(It.IsAny<double>(), It.IsAny<string>(),
                                                   It.IsAny<double>(), It.IsAny<string>()))
                        .Returns(SuccessResponse);

            _controller.DivideLength(10.0, "Feet", 2.0, "Feet");

            _mockService.Verify(s => s.DivideLength(10.0, "Feet", 2.0, "Feet"), Times.Once);
        }

        [Test]
        public void AddLengthWithTarget_DelegatesToService()
        {
            _mockService.Setup(s => s.AddLengthWithTarget(It.IsAny<double>(), It.IsAny<string>(),
                                                          It.IsAny<double>(), It.IsAny<string>(),
                                                          It.IsAny<string>()))
                        .Returns(SuccessResponse);

            _controller.AddLengthWithTarget(1.0, "Feet", 12.0, "Inches", "Inches");

            _mockService.Verify(s => s.AddLengthWithTarget(1.0, "Feet", 12.0, "Inches", "Inches"), Times.Once);
        }

        // Weight

        [Test]
        public void AddWeight_DelegatesToService()
        {
            _mockService.Setup(s => s.AddWeight(It.IsAny<double>(), It.IsAny<string>(),
                                                It.IsAny<double>(), It.IsAny<string>()))
                        .Returns(SuccessResponse);

            _controller.AddWeight(1.0, "Kilogram", 500.0, "Gram");

            _mockService.Verify(s => s.AddWeight(1.0, "Kilogram", 500.0, "Gram"), Times.Once);
        }

        [Test]
        public void ConvertWeight_DelegatesToService()
        {
            _mockService.Setup(s => s.ConvertWeight(It.IsAny<double>(), It.IsAny<string>(), It.IsAny<string>()))
                        .Returns(SuccessResponse);

            _controller.ConvertWeight(1.0, "Kilogram", "Gram");

            _mockService.Verify(s => s.ConvertWeight(1.0, "Kilogram", "Gram"), Times.Once);
        }

        [Test]
        public void CompareWeight_DelegatesToService()
        {
            _mockService.Setup(s => s.CompareWeight(It.IsAny<double>(), It.IsAny<string>(),
                                                    It.IsAny<double>(), It.IsAny<string>()))
                        .Returns(SuccessResponse);

            _controller.CompareWeight(1.0, "Kilogram", 1000.0, "Gram");

            _mockService.Verify(s => s.CompareWeight(1.0, "Kilogram", 1000.0, "Gram"), Times.Once);
        }

        [Test]
        public void SubtractWeight_DelegatesToService()
        {
            _mockService.Setup(s => s.SubtractWeight(It.IsAny<double>(), It.IsAny<string>(),
                                                     It.IsAny<double>(), It.IsAny<string>()))
                        .Returns(SuccessResponse);

            _controller.SubtractWeight(5.0, "Kilogram", 2.0, "Kilogram");

            _mockService.Verify(s => s.SubtractWeight(5.0, "Kilogram", 2.0, "Kilogram"), Times.Once);
        }

        [Test]
        public void DivideWeight_DelegatesToService()
        {
            _mockService.Setup(s => s.DivideWeight(It.IsAny<double>(), It.IsAny<string>(),
                                                   It.IsAny<double>(), It.IsAny<string>()))
                        .Returns(SuccessResponse);

            _controller.DivideWeight(10.0, "Kilogram", 2.0, "Kilogram");

            _mockService.Verify(s => s.DivideWeight(10.0, "Kilogram", 2.0, "Kilogram"), Times.Once);
        }

        // Volume

        [Test]
        public void AddVolume_DelegatesToService()
        {
            _mockService.Setup(s => s.AddVolume(It.IsAny<double>(), It.IsAny<string>(),
                                                It.IsAny<double>(), It.IsAny<string>()))
                        .Returns(SuccessResponse);

            _controller.AddVolume(1.0, "Litre", 1.0, "Litre");

            _mockService.Verify(s => s.AddVolume(1.0, "Litre", 1.0, "Litre"), Times.Once);
        }

        [Test]
        public void ConvertVolume_DelegatesToService()
        {
            _mockService.Setup(s => s.ConvertVolume(It.IsAny<double>(), It.IsAny<string>(), It.IsAny<string>()))
                        .Returns(SuccessResponse);

            _controller.ConvertVolume(1.0, "Litre", "Millilitre");

            _mockService.Verify(s => s.ConvertVolume(1.0, "Litre", "Millilitre"), Times.Once);
        }

        [Test]
        public void CompareVolume_DelegatesToService()
        {
            _mockService.Setup(s => s.CompareVolume(It.IsAny<double>(), It.IsAny<string>(),
                                                    It.IsAny<double>(), It.IsAny<string>()))
                        .Returns(SuccessResponse);

            _controller.CompareVolume(1.0, "Litre", 1000.0, "Millilitre");

            _mockService.Verify(s => s.CompareVolume(1.0, "Litre", 1000.0, "Millilitre"), Times.Once);
        }

        [Test]
        public void SubtractVolume_DelegatesToService()
        {
            _mockService.Setup(s => s.SubtractVolume(It.IsAny<double>(), It.IsAny<string>(),
                                                     It.IsAny<double>(), It.IsAny<string>()))
                        .Returns(SuccessResponse);

            _controller.SubtractVolume(5.0, "Litre", 2.0, "Litre");

            _mockService.Verify(s => s.SubtractVolume(5.0, "Litre", 2.0, "Litre"), Times.Once);
        }

        [Test]
        public void DivideVolume_DelegatesToService()
        {
            _mockService.Setup(s => s.DivideVolume(It.IsAny<double>(), It.IsAny<string>(),
                                                   It.IsAny<double>(), It.IsAny<string>()))
                        .Returns(SuccessResponse);

            _controller.DivideVolume(10.0, "Litre", 2.0, "Litre");

            _mockService.Verify(s => s.DivideVolume(10.0, "Litre", 2.0, "Litre"), Times.Once);
        }

        // Temperature

        [Test]
        public void ConvertTemperature_DelegatesToService()
        {
            _mockService.Setup(s => s.ConvertTemperature(It.IsAny<double>(), It.IsAny<string>(), It.IsAny<string>()))
                        .Returns(SuccessResponse);

            _controller.ConvertTemperature(100.0, "Celsius", "Fahrenheit");

            _mockService.Verify(s => s.ConvertTemperature(100.0, "Celsius", "Fahrenheit"), Times.Once);
        }

        [Test]
        public void CompareTemperature_DelegatesToService()
        {
            _mockService.Setup(s => s.CompareTemperature(It.IsAny<double>(), It.IsAny<string>(),
                                                         It.IsAny<double>(), It.IsAny<string>()))
                        .Returns(SuccessResponse);

            _controller.CompareTemperature(0.0, "Celsius", 32.0, "Fahrenheit");

            _mockService.Verify(s => s.CompareTemperature(0.0, "Celsius", 32.0, "Fahrenheit"), Times.Once);
        }

        // the controller should never modify the response – just pass it through

        [Test]
        public void Controller_WhenServiceReturnsError_PassesErrorThrough()
        {
            var errorResponse = QuantityResponseDTO.ForError("Add", "bad unit");
            _mockService.Setup(s => s.AddLength(It.IsAny<double>(), It.IsAny<string>(),
                                                It.IsAny<double>(), It.IsAny<string>()))
                        .Returns(errorResponse);

            var result = _controller.AddLength(1.0, "BAD", 2.0, "Feet");

            Assert.That(result.Success,      Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("bad unit"));
        }
    }
}
