using NUnit.Framework;
using QuantityMeasurementApp;

namespace QuantityMeasurementAppTest
{
    public class QuantityMeasurementAppTests
    {
        // Yard to Yard equality
        [Test]
        public void testEquality_YardToYard_SameValue()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.YARDS);
            Quantity q2 = new Quantity(1.0, LengthUnit.YARDS);

            Assert.AreEqual(q1, q2);
        }

        [Test]
        public void testEquality_YardToYard_DifferentValue()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.YARDS);
            Quantity q2 = new Quantity(2.0, LengthUnit.YARDS);

            Assert.AreNotEqual(q1, q2);
        }

        // Yard to Feet
        [Test]
        public void testEquality_YardToFeet_EquivalentValue()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.YARDS);
            Quantity q2 = new Quantity(3.0, LengthUnit.FEET);

            Assert.AreEqual(q1, q2);
        }

        [Test]
        public void testEquality_FeetToYard_EquivalentValue()
        {
            Quantity q1 = new Quantity(3.0, LengthUnit.FEET);
            Quantity q2 = new Quantity(1.0, LengthUnit.YARDS);

            Assert.AreEqual(q1, q2);
        }

        // Yard to Inches
        [Test]
        public void testEquality_YardToInches_EquivalentValue()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.YARDS);
            Quantity q2 = new Quantity(36.0, LengthUnit.INCHES);

            Assert.AreEqual(q1, q2);
        }

        // Centimeters to Inches
        [Test]
        public void testEquality_CentimetersToInches_EquivalentValue()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.CENTIMETERS);
            Quantity q2 = new Quantity(0.393701, LengthUnit.INCHES);

            Assert.AreEqual(q1, q2);
        }

        [Test]
        public void testEquality_CentimetersToFeet_NonEquivalentValue()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.CENTIMETERS);
            Quantity q2 = new Quantity(1.0, LengthUnit.FEET);

            Assert.AreNotEqual(q1, q2);
        }

        // Transitive property
        [Test]
        public void testEquality_MultiUnit_TransitiveProperty()
        {
            Quantity yard = new Quantity(1.0, LengthUnit.YARDS);
            Quantity feet = new Quantity(3.0, LengthUnit.FEET);
            Quantity inches = new Quantity(36.0, LengthUnit.INCHES);

            Assert.AreEqual(yard, feet);
            Assert.AreEqual(feet, inches);
            Assert.AreEqual(yard, inches);
        }

        // Reflexive property
        [Test]
        public void testEquality_SameReference()
        {
            Quantity q = new Quantity(2.0, LengthUnit.YARDS);

            Assert.AreEqual(q, q);
        }

        // Null comparison
        [Test]
        public void testEquality_NullComparison()
        {
            Quantity q = new Quantity(2.0, LengthUnit.YARDS);

            Assert.AreNotEqual(q, null);
        }

        // Complex scenario
        [Test]
        public void testEquality_AllUnits_ComplexScenario()
        {
            Quantity yard = new Quantity(2.0, LengthUnit.YARDS);
            Quantity feet = new Quantity(6.0, LengthUnit.FEET);
            Quantity inches = new Quantity(72.0, LengthUnit.INCHES);

            Assert.AreEqual(yard, feet);
            Assert.AreEqual(feet, inches);
            Assert.AreEqual(yard, inches);
        }
    }
}