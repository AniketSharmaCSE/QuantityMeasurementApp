using System;
using NUnit.Framework;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Units;

namespace QuantityMeasurementAppTest
{
    public class QuantityWeightTests
    {

        [Test]
        public void testEquality_KilogramToKilogram_SameValue()
        {
            QuantityWeight q1 = new QuantityWeight(1.0, WeightUnit.Kilogram);
            QuantityWeight q2 = new QuantityWeight(1.0, WeightUnit.Kilogram);

            Assert.That(q1, Is.EqualTo(q2));
        }

        [Test]
        public void testEquality_KilogramToKilogram_DifferentValue()
        {
            QuantityWeight q1 = new QuantityWeight(1.0, WeightUnit.Kilogram);
            QuantityWeight q2 = new QuantityWeight(2.0, WeightUnit.Kilogram);

            Assert.That(q1, Is.Not.EqualTo(q2));
        }

        [Test]
        public void testEquality_GramToGram_SameValue()
        {
            QuantityWeight q1 = new QuantityWeight(500.0, WeightUnit.Gram);
            QuantityWeight q2 = new QuantityWeight(500.0, WeightUnit.Gram);

            Assert.That(q1, Is.EqualTo(q2));
        }

        [Test]
        public void testEquality_PoundToPound_SameValue()
        {
            QuantityWeight q1 = new QuantityWeight(2.0, WeightUnit.Pound);
            QuantityWeight q2 = new QuantityWeight(2.0, WeightUnit.Pound);

            Assert.That(q1, Is.EqualTo(q2));
        }

        [Test]
        public void testEquality_KilogramToGram_Equivalent()
        {
            QuantityWeight q1 = new QuantityWeight(1.0, WeightUnit.Kilogram);
            QuantityWeight q2 = new QuantityWeight(1000.0, WeightUnit.Gram);

            Assert.That(q1, Is.EqualTo(q2));
        }

        [Test]
        public void testEquality_GramToKilogram_Equivalent()
        {
            QuantityWeight q1 = new QuantityWeight(1000.0, WeightUnit.Gram);
            QuantityWeight q2 = new QuantityWeight(1.0, WeightUnit.Kilogram);

            Assert.That(q1, Is.EqualTo(q2));
        }

        [Test]
        public void testEquality_KilogramToPound_Equivalent()
        {
            QuantityWeight q1 = new QuantityWeight(1.0, WeightUnit.Kilogram);
            QuantityWeight q2 = new QuantityWeight(2.20462, WeightUnit.Pound);

            Assert.That(q1, Is.EqualTo(q2));
        }

        [Test]
        public void testEquality_GramToPound_Equivalent()
        {
            QuantityWeight q1 = new QuantityWeight(453.592, WeightUnit.Gram);
            QuantityWeight q2 = new QuantityWeight(1.0, WeightUnit.Pound);

            Assert.That(q1, Is.EqualTo(q2));
        }

        [Test]
        public void testEquality_SameReference()
        {
            QuantityWeight q = new QuantityWeight(2.0, WeightUnit.Kilogram);

            Assert.That(q, Is.EqualTo(q));
        }

        [Test]
        public void testEquality_NullComparison()
        {
            QuantityWeight q = new QuantityWeight(2.0, WeightUnit.Kilogram);

            Assert.That(q, Is.Not.EqualTo(null));
        }

        [Test]
        public void testEquality_TransitiveProperty()
        {
            QuantityWeight kg = new QuantityWeight(1.0, WeightUnit.Kilogram);
            QuantityWeight g = new QuantityWeight(1000.0, WeightUnit.Gram);
            QuantityWeight lb = new QuantityWeight(2.20462, WeightUnit.Pound);

            Assert.That(kg, Is.EqualTo(g));
            Assert.That(g, Is.EqualTo(lb));
            Assert.That(kg, Is.EqualTo(lb));
        }


        [Test]
        public void testEquality_ZeroValue()
        {
            QuantityWeight q1 = new QuantityWeight(0.0, WeightUnit.Kilogram);
            QuantityWeight q2 = new QuantityWeight(0.0, WeightUnit.Gram);

            Assert.That(q1, Is.EqualTo(q2));
        }

        [Test]
        public void testEquality_NegativeWeight()
        {
            QuantityWeight q1 = new QuantityWeight(-1.0, WeightUnit.Kilogram);
            QuantityWeight q2 = new QuantityWeight(-1000.0, WeightUnit.Gram);

            Assert.That(q1, Is.EqualTo(q2));
        }

        [Test]
        public void testEquality_LargeWeightValue()
        {
            QuantityWeight q1 = new QuantityWeight(1000000.0, WeightUnit.Gram);
            QuantityWeight q2 = new QuantityWeight(1000.0, WeightUnit.Kilogram);

            Assert.That(q1, Is.EqualTo(q2));
        }

        [Test]
        public void testEquality_SmallWeightValue()
        {
            QuantityWeight q1 = new QuantityWeight(0.001, WeightUnit.Kilogram);
            QuantityWeight q2 = new QuantityWeight(1.0, WeightUnit.Gram);

            Assert.That(q1, Is.EqualTo(q2));
        }

        [Test]
        public void testConversion_KilogramToGram()
        {
            QuantityWeight q = new QuantityWeight(1.0, WeightUnit.Kilogram);

            QuantityWeight result = q.ConvertTo(WeightUnit.Gram);

            Assert.That(result.GetValue(), Is.EqualTo(1000.0));
        }

        [Test]
        public void testConversion_PoundToKilogram()
        {
            QuantityWeight q = new QuantityWeight(2.20462, WeightUnit.Pound);

            QuantityWeight result = q.ConvertTo(WeightUnit.Kilogram);

            Assert.That(result.GetValue(), Is.EqualTo(1.0).Within(1e-3));
        }

        [Test]
        public void testConversion_KilogramToPound()
        {
            QuantityWeight q = new QuantityWeight(1.0, WeightUnit.Kilogram);

            QuantityWeight result = q.ConvertTo(WeightUnit.Pound);

            Assert.That(result.GetValue(), Is.EqualTo(2.20462).Within(1e-3));
        }

        [Test]
        public void testConversion_ZeroValue()
        {
            QuantityWeight q = new QuantityWeight(0.0, WeightUnit.Kilogram);

            QuantityWeight result = q.ConvertTo(WeightUnit.Gram);

            Assert.That(result.GetValue(), Is.EqualTo(0.0));
        }

        [Test]
        public void testConversion_NegativeValue()
        {
            QuantityWeight q = new QuantityWeight(-1.0, WeightUnit.Kilogram);

            QuantityWeight result = q.ConvertTo(WeightUnit.Gram);

            Assert.That(result.GetValue(), Is.EqualTo(-1000.0));
        }

        [Test]
        public void testConversion_RoundTrip()
        {
            QuantityWeight q = new QuantityWeight(1.5, WeightUnit.Kilogram);

            QuantityWeight g = q.ConvertTo(WeightUnit.Gram);
            QuantityWeight back = g.ConvertTo(WeightUnit.Kilogram);

            Assert.That(back.GetValue(), Is.EqualTo(1.5).Within(1e-6));
        }

        [Test]
        public void testAddition_SameUnit_KilogramPlusKilogram()
        {
            QuantityWeight q1 = new QuantityWeight(1.0, WeightUnit.Kilogram);
            QuantityWeight q2 = new QuantityWeight(2.0, WeightUnit.Kilogram);

            QuantityWeight result = q1.Add(q2);

            Assert.That(result, Is.EqualTo(new QuantityWeight(3.0, WeightUnit.Kilogram)));
        }

        [Test]
        public void testAddition_CrossUnit_KilogramPlusGram()
        {
            QuantityWeight q1 = new QuantityWeight(1.0, WeightUnit.Kilogram);
            QuantityWeight q2 = new QuantityWeight(1000.0, WeightUnit.Gram);

            QuantityWeight result = q1.Add(q2);

            Assert.That(result, Is.EqualTo(new QuantityWeight(2.0, WeightUnit.Kilogram)));
        }

        [Test]
        public void testAddition_CrossUnit_PoundPlusKilogram()
        {
            QuantityWeight q1 = new QuantityWeight(2.20462, WeightUnit.Pound);
            QuantityWeight q2 = new QuantityWeight(1.0, WeightUnit.Kilogram);

            QuantityWeight result = q1.Add(q2);

            Assert.That(result.GetValue(), Is.EqualTo(4.40924).Within(1e-2));
        }

        [Test]
        public void testAddition_WithZero()
        {
            QuantityWeight q1 = new QuantityWeight(5.0, WeightUnit.Kilogram);
            QuantityWeight q2 = new QuantityWeight(0.0, WeightUnit.Gram);

            QuantityWeight result = q1.Add(q2);

            Assert.That(result, Is.EqualTo(new QuantityWeight(5.0, WeightUnit.Kilogram)));
        }

        [Test]
        public void testAddition_NegativeValues()
        {
            QuantityWeight q1 = new QuantityWeight(5.0, WeightUnit.Kilogram);
            QuantityWeight q2 = new QuantityWeight(-2000.0, WeightUnit.Gram);

            QuantityWeight result = q1.Add(q2);

            Assert.That(result, Is.EqualTo(new QuantityWeight(3.0, WeightUnit.Kilogram)));
        }

        [Test]
        public void testAddition_LargeValues()
        {
            QuantityWeight q1 = new QuantityWeight(1e6, WeightUnit.Kilogram);
            QuantityWeight q2 = new QuantityWeight(1e6, WeightUnit.Kilogram);

            QuantityWeight result = q1.Add(q2);

            Assert.That(result, Is.EqualTo(new QuantityWeight(2e6, WeightUnit.Kilogram)));
        }

        [Test]
        public void testAddition_NullSecondOperand()
        {
            QuantityWeight q1 = new QuantityWeight(1.0, WeightUnit.Kilogram);

            Assert.Throws<ArgumentException>(() => q1.Add(null));
        }

    }
}