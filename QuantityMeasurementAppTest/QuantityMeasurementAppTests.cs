using NUnit.Framework;
using QuantityMeasurementApp;

namespace QuantityMeasurementAppTests
{
    [TestFixture]
    public class QuantityMeasurementAppTest
    {
        //Feet tests

        [Test]
        public void TestFeetEquality_SameValue()
        {
            Feet feet1 = new Feet(1.0);
            Feet feet2 = new Feet(1.0);

            bool result = feet1.Equals(feet2);

            Assert.That(result, Is.True);
        }

        [Test]
        public void TestFeetEquality_DifferentValue()
        {
            Feet feet1 = new Feet(1.0);
            Feet feet2 = new Feet(2.0);

            bool result = feet1.Equals(feet2);

            Assert.That(result, Is.False);
        }

        [Test]
        public void TestFeetEquality_NullComparison()
        {
            Feet feet = new Feet(1.0);

            bool result = feet.Equals(null);

            Assert.That(result, Is.False);
        }

        [Test]
        public void TestFeetEquality_TypeMismatch()
        {
            Feet feet = new Feet(1.0);
            object other = new object();

            bool result = feet.Equals(other);

            Assert.That(result, Is.False);
        }

        [Test]
        public void TestFeetEquality_SameReference()
        {
            Feet feet = new Feet(1.0);

            bool result = feet.Equals(feet);

            Assert.That(result, Is.True);
        }

        //Inches Tests

        [Test]
        public void TestInchesEquality_SameValue()
        {
            Inches inch1 = new Inches(1.0);
            Inches inch2 = new Inches(1.0);

            bool result = inch1.Equals(inch2);

            Assert.That(result, Is.True);
        }

        [Test]
        public void TestInchesEquality_DifferentValue()
        {
            Inches inch1 = new Inches(1.0);
            Inches inch2 = new Inches(2.0);

            bool result = inch1.Equals(inch2);

            Assert.That(result, Is.False);
        }

        [Test]
        public void TestInchesEquality_NullComparison()
        {
            Inches inch = new Inches(1.0);

            bool result = inch.Equals(null);

            Assert.That(result, Is.False);
        }

        [Test]
        public void TestInchesEquality_TypeMismatch()
        {
            Inches inch = new Inches(1.0);
            object other = new object();

            bool result = inch.Equals(other);

            Assert.That(result, Is.False);
        }

        [Test]
        public void TestInchesEquality_SameReference()
        {
            Inches inch = new Inches(1.0);

            bool result = inch.Equals(inch);

            Assert.That(result, Is.True);
        }
    }
}