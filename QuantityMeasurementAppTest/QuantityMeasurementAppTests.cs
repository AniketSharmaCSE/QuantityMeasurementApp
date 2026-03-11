using System;
using NUnit.Framework;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Units;

namespace QuantityMeasurementAppTest
{
    public class QuantityMeasurementAppTests
    {

        // length equality tests

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

        // length conversion tests

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

        // length addition tests

        [Test]
        public void testAddition_SameUnit_FeetPlusFeet()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(2.0, LengthUnit.Feet);

            var result = q1.Add(q2);

            Assert.That(result.GetValue(), Is.EqualTo(3.0));
        }

        [Test]
        public void testAddition_CrossUnit_FeetPlusInches()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12.0, LengthUnit.Inches);

            var result = q1.Add(q2);

            Assert.That(result.GetValue(), Is.EqualTo(2.0));
        }

        [Test]
        public void testAddition_WithZero()
        {
            var q1 = new Quantity<LengthUnit>(5.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(0.0, LengthUnit.Inches);

            var result = q1.Add(q2);

            Assert.That(result.GetValue(), Is.EqualTo(5.0));
        }

        // weight tests

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
        public void testAddition_KilogramPlusGram()
        {
            var q1 = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);
            var q2 = new Quantity<WeightUnit>(1000.0, WeightUnit.Gram);

            var result = q1.Add(q2, WeightUnit.Kilogram);

            Assert.That(result.GetValue(), Is.EqualTo(2.0).Within(1e-4));
        }

        // volume equality tests

        [Test]
        public void testEquality_LitreToLitre_SameValue()
        {
            var q1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var q2 = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);

            Assert.That(q1, Is.EqualTo(q2));
        }

        [Test]
        public void testEquality_LitreToMillilitre_EquivalentValue()
        {
            var q1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var q2 = new Quantity<VolumeUnit>(1000.0, VolumeUnit.Millilitre);

            Assert.That(q1, Is.EqualTo(q2));
        }

        [Test]
        public void testEquality_LitreToGallon_EquivalentValue()
        {
            var q1 = new Quantity<VolumeUnit>(3.78541, VolumeUnit.Litre);
            var q2 = new Quantity<VolumeUnit>(1.0, VolumeUnit.Gallon);

            Assert.That(q1, Is.EqualTo(q2));
        }

        // volume conversion tests

        [Test]
        public void testConversion_LitreToMillilitre()
        {
            var q = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);

            var result = q.ConvertTo(VolumeUnit.Millilitre);

            Assert.That(result.GetValue(), Is.EqualTo(1000.0).Within(1e-6));
        }

        [Test]
        public void testConversion_MillilitreToLitre()
        {
            var q = new Quantity<VolumeUnit>(1000.0, VolumeUnit.Millilitre);

            var result = q.ConvertTo(VolumeUnit.Litre);

            Assert.That(result.GetValue(), Is.EqualTo(1.0).Within(1e-6));
        }

        [Test]
        public void testConversion_GallonToLitre()
        {
            var q = new Quantity<VolumeUnit>(1.0, VolumeUnit.Gallon);

            var result = q.ConvertTo(VolumeUnit.Litre);

            Assert.That(result.GetValue(), Is.EqualTo(3.78541).Within(1e-4));
        }

        // volume addition tests

        [Test]
        public void testAddition_SameUnit_LitrePlusLitre()
        {
            var q1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var q2 = new Quantity<VolumeUnit>(2.0, VolumeUnit.Litre);

            var result = q1.Add(q2);

            Assert.That(result.GetValue(), Is.EqualTo(3.0));
        }

        [Test]
        public void testAddition_CrossUnit_LitrePlusMillilitre()
        {
            var q1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var q2 = new Quantity<VolumeUnit>(1000.0, VolumeUnit.Millilitre);

            var result = q1.Add(q2);

            Assert.That(result.GetValue(), Is.EqualTo(2.0));
        }

        [Test]
        public void testAddition_ExplicitTargetUnit_Millilitre()
        {
            var q1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var q2 = new Quantity<VolumeUnit>(1000.0, VolumeUnit.Millilitre);

            var result = q1.Add(q2, VolumeUnit.Millilitre);

            Assert.That(result.GetValue(), Is.EqualTo(2000.0));
        }

        // cross category tests

        [Test]
        public void testCrossCategoryPrevention_VolumeVsLength()
        {
            var volume = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var length = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);

            Assert.That(volume.Equals(length), Is.False);
        }

        [Test]
        public void testCrossCategoryPrevention_VolumeVsWeight()
        {
            var volume = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var weight = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);

            Assert.That(volume.Equals(weight), Is.False);
        }

        // constructor validation

        [Test]
        public void testConstructor_InvalidValue_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                new Quantity<VolumeUnit>(double.NaN, VolumeUnit.Litre));
        }

    }
}