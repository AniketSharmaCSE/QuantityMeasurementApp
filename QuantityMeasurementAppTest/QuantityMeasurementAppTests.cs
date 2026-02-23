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


        [Test]
        public void testConversion_FeetToInches()
        {
            double result = Quantity.Convert(1.0, LengthUnit.Feet, LengthUnit.Inches);

            Assert.That(result, Is.EqualTo(12.0).Within(1e-6));
        }

        [Test]
        public void testConversion_InchesToFeet()
        {
            double result = Quantity.Convert(24.0, LengthUnit.Inches, LengthUnit.Feet);

            Assert.That(result, Is.EqualTo(2.0).Within(1e-6));
        }

        [Test]
        public void testConversion_YardsToInches()
        {
            double result = Quantity.Convert(1.0, LengthUnit.Yards, LengthUnit.Inches);

            Assert.That(result, Is.EqualTo(36.0).Within(1e-6));
        }

        [Test]
        public void testConversion_InchesToYards()
        {
            double result = Quantity.Convert(72.0, LengthUnit.Inches, LengthUnit.Yards);

            Assert.That(result, Is.EqualTo(2.0).Within(1e-6));
        }

        [Test]
        public void testConversion_CentimetersToInches()
        {
            double result = Quantity.Convert(2.54, LengthUnit.Centimeters, LengthUnit.Inches);

            Assert.That(result, Is.EqualTo(1.0).Within(1e-4));
        }

        [Test]
        public void testConversion_FeetToYards()
        {
            double result = Quantity.Convert(6.0, LengthUnit.Feet, LengthUnit.Yards);

            Assert.That(result, Is.EqualTo(2.0).Within(1e-6));
        }

        [Test]
        public void testConversion_RoundTrip_PreservesValue()
        {
            double original = 5.0;

            double converted = Quantity.Convert(original, LengthUnit.Feet, LengthUnit.Inches);
            double back = Quantity.Convert(converted, LengthUnit.Inches, LengthUnit.Feet);

            Assert.That(back, Is.EqualTo(original).Within(1e-6));
        }

        [Test]
        public void testConversion_ZeroValue()
        {
            double result = Quantity.Convert(0.0, LengthUnit.Feet, LengthUnit.Inches);

            Assert.That(result, Is.EqualTo(0.0));
        }

        [Test]
        public void testConversion_NegativeValue()
        {
            double result = Quantity.Convert(-1.0, LengthUnit.Feet, LengthUnit.Inches);

            Assert.That(result, Is.EqualTo(-12.0).Within(1e-6));
        }

        [Test]
        public void testConversion_InvalidUnit_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                Quantity.Convert(1.0, (LengthUnit)999, LengthUnit.Feet));
        }

        [Test]
        public void testConversion_NaN_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                Quantity.Convert(double.NaN, LengthUnit.Feet, LengthUnit.Inches));
        }

        [Test]
        public void testConversion_Infinity_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                Quantity.Convert(double.PositiveInfinity, LengthUnit.Feet, LengthUnit.Inches));
        }

        [Test]
        public void testConversion_PrecisionTolerance()
        {
            double result = Quantity.Convert(1.0, LengthUnit.Feet, LengthUnit.Centimeters);

            Assert.That(result, Is.EqualTo(30.48).Within(1e-2));
        }

        //UC6 ADDITION TESTS

        [Test]
        public void testAddition_SameUnit_FeetPlusFeet()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.Feet);
            Quantity q2 = new Quantity(2.0, LengthUnit.Feet);

            Quantity result = q1.Add(q2);

            Assert.That(result, Is.EqualTo(new Quantity(3.0, LengthUnit.Feet)));
        }

        [Test]
        public void testAddition_CrossUnit_FeetPlusInches()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.Feet);
            Quantity q2 = new Quantity(12.0, LengthUnit.Inches);

            Quantity result = q1.Add(q2);

            Assert.That(result, Is.EqualTo(new Quantity(2.0, LengthUnit.Feet)));
        }

        [Test]
        public void testAddition_CrossUnit_InchesPlusFeet()
        {
            Quantity q1 = new Quantity(12.0, LengthUnit.Inches);
            Quantity q2 = new Quantity(1.0, LengthUnit.Feet);

            Quantity result = q1.Add(q2);

            Assert.That(result, Is.EqualTo(new Quantity(24.0, LengthUnit.Inches)));
        }

        [Test]
        public void testAddition_YardsPlusFeet()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.Yards);
            Quantity q2 = new Quantity(3.0, LengthUnit.Feet);

            Quantity result = q1.Add(q2);

            Assert.That(result, Is.EqualTo(new Quantity(2.0, LengthUnit.Yards)));
        }

        [Test]
        public void testAddition_CentimeterPlusInch()
        {
            Quantity q1 = new Quantity(2.54, LengthUnit.Centimeters);
            Quantity q2 = new Quantity(1.0, LengthUnit.Inches);

            Quantity result = q1.Add(q2);

            Assert.That(result, Is.EqualTo(new Quantity(5.08, LengthUnit.Centimeters)));
        }

        [Test]
        public void testAddition_Commutativity()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.Feet);
            Quantity q2 = new Quantity(12.0, LengthUnit.Inches);

            Quantity result1 = q1.Add(q2);
            Quantity result2 = q2.Add(q1);

            Assert.That(result1, Is.EqualTo(result2));
        }

        [Test]
        public void testAddition_WithZero()
        {
            Quantity q1 = new Quantity(5.0, LengthUnit.Feet);
            Quantity q2 = new Quantity(0.0, LengthUnit.Inches);

            Quantity result = q1.Add(q2);

            Assert.That(result, Is.EqualTo(new Quantity(5.0, LengthUnit.Feet)));
        }

        [Test]
        public void testAddition_NegativeValues()
        {
            Quantity q1 = new Quantity(5.0, LengthUnit.Feet);
            Quantity q2 = new Quantity(-2.0, LengthUnit.Feet);

            Quantity result = q1.Add(q2);

            Assert.That(result, Is.EqualTo(new Quantity(3.0, LengthUnit.Feet)));
        }

        [Test]
        public void testAddition_NullSecondOperand()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.Feet);

            Assert.Throws<ArgumentException>(() => q1.Add(null));
        }

        [Test]
        public void testAddition_LargeValues()
        {
            Quantity q1 = new Quantity(1e6, LengthUnit.Feet);
            Quantity q2 = new Quantity(1e6, LengthUnit.Feet);

            Quantity result = q1.Add(q2);

            Assert.That(result, Is.EqualTo(new Quantity(2e6, LengthUnit.Feet)));
        }

        [Test]
        public void testAddition_SmallValues()
        {
            Quantity q1 = new Quantity(0.001, LengthUnit.Feet);
            Quantity q2 = new Quantity(0.002, LengthUnit.Feet);

            Quantity result = q1.Add(q2);

            Assert.That(result, Is.EqualTo(new Quantity(0.003, LengthUnit.Feet)));
        }
        
        // UC7 EXPLICIT TARGET UNIT TESTS

        [Test]
        public void testAddition_ExplicitTargetUnit_Feet()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.Feet);
            Quantity q2 = new Quantity(12.0, LengthUnit.Inches);

            Quantity result = q1.Add(q2, LengthUnit.Feet);

            Assert.That(result, Is.EqualTo(new Quantity(2.0, LengthUnit.Feet)));
        }

        [Test]
        public void testAddition_ExplicitTargetUnit_Inches()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.Feet);
            Quantity q2 = new Quantity(12.0, LengthUnit.Inches);

            Quantity result = q1.Add(q2, LengthUnit.Inches);

            Assert.That(result, Is.EqualTo(new Quantity(24.0, LengthUnit.Inches)));
        }

        [Test]
        public void testAddition_ExplicitTargetUnit_Yards()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.Feet);
            Quantity q2 = new Quantity(12.0, LengthUnit.Inches);

            Quantity result = q1.Add(q2, LengthUnit.Yards);

            Quantity expected = new Quantity(2.0, LengthUnit.Feet).ConvertTo(LengthUnit.Yards);
            Assert.That(result, Is.EqualTo(expected));       
        }

        [Test]
        public void testAddition_ExplicitTargetUnit_Commutativity()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.Feet);
            Quantity q2 = new Quantity(12.0, LengthUnit.Inches);

            Quantity result1 = q1.Add(q2, LengthUnit.Yards);
            Quantity result2 = q2.Add(q1, LengthUnit.Yards);

            Assert.That(result1, Is.EqualTo(result2));
        }

        [Test]
        public void testAddition_ExplicitTargetUnit_WithZero()
        {
            Quantity q1 = new Quantity(5.0, LengthUnit.Feet);
            Quantity q2 = new Quantity(0.0, LengthUnit.Inches);

            Quantity result = q1.Add(q2, LengthUnit.Yards);

            Quantity expected = new Quantity(5.0, LengthUnit.Feet).Add(new Quantity(0.0, LengthUnit.Inches)).ConvertTo(LengthUnit.Yards);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void testAddition_ExplicitTargetUnit_NegativeValues()
        {
            Quantity q1 = new Quantity(5.0, LengthUnit.Feet);
            Quantity q2 = new Quantity(-2.0, LengthUnit.Feet);

            Quantity result = q1.Add(q2, LengthUnit.Inches);

            Assert.That(result, Is.EqualTo(new Quantity(36.0, LengthUnit.Inches)));
        }

        [Test]
        public void testAddition_ExplicitTargetUnit_NullSecondOperand()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.Feet);

            Assert.Throws<ArgumentException>(() =>
                q1.Add(null, LengthUnit.Feet));
        }
    }
}
