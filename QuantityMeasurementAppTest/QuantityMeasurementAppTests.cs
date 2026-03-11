using System;
using NUnit.Framework;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Units;

namespace QuantityMeasurementAppTest
{
    public class QuantityMeasurementAppTests
    {
        // Length Equality

        [Test]
        public void testEquality_YardToYard_SameValue()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Yards);
            var q2 = new Quantity<LengthUnit>(1.0, LengthUnit.Yards);

            Assert.That(q1, Is.EqualTo(q2));
        }

        [Test]
        public void testEquality_YardToYard_DifferentValue()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Yards);
            var q2 = new Quantity<LengthUnit>(2.0, LengthUnit.Yards);

            Assert.That(q1, Is.Not.EqualTo(q2));
        }

        [Test]
        public void testEquality_YardToFeet_EquivalentValue()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Yards);
            var q2 = new Quantity<LengthUnit>(3.0, LengthUnit.Feet);

            Assert.That(q1, Is.EqualTo(q2));
        }

        [Test]
        public void testEquality_YardToInches_EquivalentValue()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Yards);
            var q2 = new Quantity<LengthUnit>(36.0, LengthUnit.Inches);

            Assert.That(q1, Is.EqualTo(q2));
        }

        [Test]
        public void testEquality_MultiUnit_TransitiveProperty()
        {
            var yard = new Quantity<LengthUnit>(1.0, LengthUnit.Yards);
            var feet = new Quantity<LengthUnit>(3.0, LengthUnit.Feet);
            var inches = new Quantity<LengthUnit>(36.0, LengthUnit.Inches);

            Assert.That(yard, Is.EqualTo(feet));
            Assert.That(feet, Is.EqualTo(inches));
            Assert.That(yard, Is.EqualTo(inches));
        }

        [Test]
        public void testEquality_SameReference()
        {
            var q = new Quantity<LengthUnit>(2.0, LengthUnit.Yards);

            Assert.That(q, Is.EqualTo(q));
        }

        [Test]
        public void testEquality_NullComparison()
        {
            var q = new Quantity<LengthUnit>(2.0, LengthUnit.Yards);

            Assert.That(q, Is.Not.EqualTo(null));
        }

        // Length Conversion

        [Test]
        public void testConversion_FeetToInches()
        {
            var q = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);

            var result = q.ConvertTo(LengthUnit.Inches);

            Assert.That(result.GetValue(), Is.EqualTo(12.0).Within(1e-6));
        }

        [Test]
        public void testConversion_InchesToFeet()
        {
            var q = new Quantity<LengthUnit>(24.0, LengthUnit.Inches);

            var result = q.ConvertTo(LengthUnit.Feet);

            Assert.That(result.GetValue(), Is.EqualTo(2.0).Within(1e-6));
        }

        [Test]
        public void testConversion_YardsToInches()
        {
            var q = new Quantity<LengthUnit>(1.0, LengthUnit.Yards);

            var result = q.ConvertTo(LengthUnit.Inches);

            Assert.That(result.GetValue(), Is.EqualTo(36.0).Within(1e-6));
        }

        [Test]
        public void testConversion_RoundTrip_PreservesValue()
        {
            var q = new Quantity<LengthUnit>(5.0, LengthUnit.Feet);

            var inches = q.ConvertTo(LengthUnit.Inches);
            var back = inches.ConvertTo(LengthUnit.Feet);

            Assert.That(back.GetValue(), Is.EqualTo(5.0).Within(1e-6));
        }

        [Test]
        public void testConversion_ZeroValue()
        {
            var q = new Quantity<LengthUnit>(0.0, LengthUnit.Feet);

            var result = q.ConvertTo(LengthUnit.Inches);

            Assert.That(result.GetValue(), Is.EqualTo(0.0));
        }

        [Test]
        public void testConversion_NegativeValue()
        {
            var q = new Quantity<LengthUnit>(-1.0, LengthUnit.Feet);

            var result = q.ConvertTo(LengthUnit.Inches);

            Assert.That(result.GetValue(), Is.EqualTo(-12.0).Within(1e-6));
        }

        // Length Addition

        [Test]
        public void testAddition_SameUnit_FeetPlusFeet()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(2.0, LengthUnit.Feet);

            var result = q1.Add(q2);

            Assert.That(result, Is.EqualTo(new Quantity<LengthUnit>(3.0, LengthUnit.Feet)));
        }

        [Test]
        public void testAddition_CrossUnit_FeetPlusInches()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12.0, LengthUnit.Inches);

            var result = q1.Add(q2);

            Assert.That(result, Is.EqualTo(new Quantity<LengthUnit>(2.0, LengthUnit.Feet)));
        }

        [Test]
        public void testAddition_Commutativity()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12.0, LengthUnit.Inches);

            var result1 = q1.Add(q2);
            var result2 = q2.Add(q1);

            Assert.That(result1, Is.EqualTo(result2));
        }

        [Test]
        public void testAddition_WithZero()
        {
            var q1 = new Quantity<LengthUnit>(5.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(0.0, LengthUnit.Inches);

            var result = q1.Add(q2);

            Assert.That(result.GetValue(), Is.EqualTo(5.0));
        }

        [Test]
        public void testAddition_NegativeValues()
        {
            var q1 = new Quantity<LengthUnit>(5.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(-2.0, LengthUnit.Feet);

            var result = q1.Add(q2);

            Assert.That(result.GetValue(), Is.EqualTo(3.0));
        }

        [Test]
        public void testAddition_NullSecondOperand()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);

            Assert.Throws<ArgumentException>(() => q1.Add(null));
        }

        // Weight Tests

        [Test]
        public void testEquality_KilogramToGram()
        {
            var q1 = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);
            var q2 = new Quantity<WeightUnit>(1000.0, WeightUnit.Gram);

            Assert.That(q1, Is.EqualTo(q2));
        }

        [Test]
        public void testConversion_KilogramToGram()
        {
            var q = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);

            var result = q.ConvertTo(WeightUnit.Gram);

            Assert.That(result.GetValue(), Is.EqualTo(1000.0).Within(1e-4));
        }

        [Test]
        public void testConversion_PoundToKilogram()
        {
            var q = new Quantity<WeightUnit>(2.20462, WeightUnit.Pound);

            var result = q.ConvertTo(WeightUnit.Kilogram);

            Assert.That(result.GetValue(), Is.EqualTo(1.0).Within(1e-2));
        }

        [Test]
        public void testAddition_KilogramPlusGram()
        {
            var q1 = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);
            var q2 = new Quantity<WeightUnit>(1000.0, WeightUnit.Gram);

            var result = q1.Add(q2, WeightUnit.Kilogram);

            Assert.That(result.GetValue(), Is.EqualTo(2.0).Within(1e-4));
        }

        // Cross Category

        [Test]
        public void testCrossCategoryPrevention_LengthVsWeight()
        {
            var length = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var weight = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);

            Assert.That(length.Equals(weight), Is.False);
        }

        // Constructor Validation

        [Test]
        public void testConstructor_InvalidValue_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                new Quantity<LengthUnit>(double.NaN, LengthUnit.Feet));
        }

        // Hashcode

        [Test]
        public void testHashCode_GenericQuantity_Consistency()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12.0, LengthUnit.Inches);

            Assert.That(q1.Equals(q2));
            Assert.That(q1.GetHashCode(), Is.EqualTo(q2.GetHashCode()));
        }

        // Immutability
        [Test]
        public void testImmutability_GenericQuantity()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);

            var q2 = q1.ConvertTo(LengthUnit.Inches);

            Assert.That(q1.GetValue(), Is.EqualTo(1.0));
            Assert.That(q2.GetValue(), Is.EqualTo(12.0).Within(1e-4));
        }
    }
}