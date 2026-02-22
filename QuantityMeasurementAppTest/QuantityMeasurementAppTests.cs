using NUnit.Framework;
using QuantityMeasurementApp;

namespace QuantityMeasurementAppTests
{
    [TestFixture]
    public class QuantityTests
    {
        [Test]
        public void testEquality_FeetToFeet_SameValue()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.Feet);
            Quantity q2 = new Quantity(1.0, LengthUnit.Feet);

            Assert.That(q1.Equals(q2), Is.True);
        }

        [Test]
        public void testEquality_InchToInch_SameValue()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.Inches);
            Quantity q2 = new Quantity(1.0, LengthUnit.Inches);

            Assert.That(q1.Equals(q2), Is.True);
        }

        [Test]
        public void testEquality_InchToFeet_EquivalentValue()
        {
            Quantity q1 = new Quantity(12.0, LengthUnit.Inches);
            Quantity q2 = new Quantity(1.0, LengthUnit.Feet);

            Assert.That(q1.Equals(q2), Is.True);
        }

        [Test]
        public void testEquality_FeetToFeet_DifferentValue()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.Feet);
            Quantity q2 = new Quantity(2.0, LengthUnit.Feet);

            Assert.That(q1.Equals(q2), Is.False);
        }

        [Test]
        public void testEquality_InchToInch_DifferentValue()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.Inches);
            Quantity q2 = new Quantity(2.0, LengthUnit.Inches);

            Assert.That(q1.Equals(q2), Is.False);
        }

        [Test]
        public void testEquality_SameReference()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.Feet);

            Assert.That(q1.Equals(q1), Is.True);
        }

        [Test]
        public void testEquality_NullComparison()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.Feet);

            Assert.That(q1.Equals(null), Is.False);
        }
    }
}