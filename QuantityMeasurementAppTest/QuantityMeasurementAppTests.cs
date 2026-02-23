using NUnit.Framework;
using QuantityMeasurementApp;

namespace QuantityMeasurementAppTest
{
    public class QuantityMeasurementAppTests
    {
        [Test]
        public void testEquality_YardToYard_SameValue()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.Yards);
            Quantity q2 = new Quantity(1.0, LengthUnit.Yards);

            Assert.That(q1, Is.EqualTo(q2));
        }

        [Test]
        public void testEquality_YardToYard_DifferentValue()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.Yards);
            Quantity q2 = new Quantity(2.0, LengthUnit.Yards);

            Assert.That(q1, Is.Not.EqualTo(q2));
        }

        [Test]
        public void testEquality_YardToFeet_EquivalentValue()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.Yards);
            Quantity q2 = new Quantity(3.0, LengthUnit.Feet);

            Assert.That(q1, Is.EqualTo(q2));
        }

        [Test]
        public void testEquality_FeetToYard_EquivalentValue()
        {
            Quantity q1 = new Quantity(3.0, LengthUnit.Feet);
            Quantity q2 = new Quantity(1.0, LengthUnit.Yards);

            Assert.That(q1, Is.EqualTo(q2));
        }

        [Test]
        public void testEquality_YardToInches_EquivalentValue()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.Yards);
            Quantity q2 = new Quantity(36.0, LengthUnit.Inches);

            Assert.That(q1, Is.EqualTo(q2));
        }

        [Test]
        public void testEquality_CentimetersToInches_EquivalentValue()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.Centimeters);
            Quantity q2 = new Quantity(0.393701, LengthUnit.Inches);

            Assert.That(q1, Is.EqualTo(q2));
        }

        [Test]
        public void testEquality_CentimetersToFeet_NonEquivalentValue()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.Centimeters);
            Quantity q2 = new Quantity(1.0, LengthUnit.Feet);

            Assert.That(q1, Is.Not.EqualTo(q2));
        }

        [Test]
        public void testEquality_MultiUnit_TransitiveProperty()
        {
            Quantity yard = new Quantity(1.0, LengthUnit.Yards);
            Quantity feet = new Quantity(3.0, LengthUnit.Feet);
            Quantity inches = new Quantity(36.0, LengthUnit.Inches);

            Assert.That(yard, Is.EqualTo(feet));
            Assert.That(feet, Is.EqualTo(inches));
            Assert.That(yard, Is.EqualTo(inches));
        }

        [Test]
        public void testEquality_SameReference()
        {
            Quantity q = new Quantity(2.0, LengthUnit.Yards);

            Assert.That(q, Is.EqualTo(q));
        }

        [Test]
        public void testEquality_NullComparison()
        {
            Quantity q = new Quantity(2.0, LengthUnit.Yards);

            Assert.That(q, Is.Not.EqualTo(null));
        }

        [Test]
        public void testEquality_AllUnits_ComplexScenario()
        {
            Quantity yard = new Quantity(2.0, LengthUnit.Yards);
            Quantity feet = new Quantity(6.0, LengthUnit.Feet);
            Quantity inches = new Quantity(72.0, LengthUnit.Inches);

            Assert.That(yard, Is.EqualTo(feet));
            Assert.That(feet, Is.EqualTo(inches));
            Assert.That(yard, Is.EqualTo(inches));
        }
    }
}