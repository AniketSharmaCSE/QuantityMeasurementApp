using System;
using NUnit.Framework;
using QuantityMeasurement.Model.Entities;
using QuantityMeasurement.Model.Units;

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

         // subtraction same unit length
        [Test]
        public void testSubtraction_SameUnit_FeetMinusFeet()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(5.0, LengthUnit.Feet);

            var result = q1.Subtract(q2);

            Assert.That(result.GetValue(), Is.EqualTo(5.0));
        }

        // subtraction same unit volume
        [Test]
        public void testSubtraction_SameUnit_LitreMinusLitre()
        {
            var q1 = new Quantity<VolumeUnit>(10.0, VolumeUnit.Litre);
            var q2 = new Quantity<VolumeUnit>(3.0, VolumeUnit.Litre);

            var result = q1.Subtract(q2);

            Assert.That(result.GetValue(), Is.EqualTo(7.0));
        }

        // subtraction cross unit
        [Test]
        public void testSubtraction_CrossUnit_FeetMinusInches()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(6.0, LengthUnit.Inches);

            var result = q1.Subtract(q2);

            Assert.That(result.GetValue(), Is.EqualTo(9.5).Within(1e-4));
        }

        // subtraction reverse units
        [Test]
        public void testSubtraction_CrossUnit_InchesMinusFeet()
        {
            var q1 = new Quantity<LengthUnit>(120.0, LengthUnit.Inches);
            var q2 = new Quantity<LengthUnit>(5.0, LengthUnit.Feet);

            var result = q1.Subtract(q2);

            Assert.That(result.GetValue(), Is.EqualTo(60.0).Within(1e-4));
        }

        // subtraction explicit unit feet
        [Test]
        public void testSubtraction_ExplicitTargetUnit_Feet()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(6.0, LengthUnit.Inches);

            var result = q1.Subtract(q2, LengthUnit.Feet);

            Assert.That(result.GetValue(), Is.EqualTo(9.5).Within(1e-4));
        }

        // subtraction explicit unit inches
        [Test]
        public void testSubtraction_ExplicitTargetUnit_Inches()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(6.0, LengthUnit.Inches);

            var result = q1.Subtract(q2, LengthUnit.Inches);

            Assert.That(result.GetValue(), Is.EqualTo(114.0).Within(1e-4));
        }

        // subtraction explicit millilitre
        [Test]
        public void testSubtraction_ExplicitTargetUnit_Millilitre()
        {
            var q1 = new Quantity<VolumeUnit>(5.0, VolumeUnit.Litre);
            var q2 = new Quantity<VolumeUnit>(2.0, VolumeUnit.Litre);

            var result = q1.Subtract(q2, VolumeUnit.Millilitre);

            Assert.That(result.GetValue(), Is.EqualTo(3000.0));
        }

        // subtraction negative result
        [Test]
        public void testSubtraction_ResultingInNegative()
        {
            var q1 = new Quantity<LengthUnit>(5.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);

            var result = q1.Subtract(q2);

            Assert.That(result.GetValue(), Is.EqualTo(-5.0));
        }

        // subtraction zero result
        [Test]
        public void testSubtraction_ResultingInZero()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(120.0, LengthUnit.Inches);

            var result = q1.Subtract(q2);

            Assert.That(result.GetValue(), Is.EqualTo(0.0).Within(1e-4));
        }

        // subtraction zero operand
        [Test]
        public void testSubtraction_WithZeroOperand()
        {
            var q1 = new Quantity<LengthUnit>(5.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(0.0, LengthUnit.Inches);

            var result = q1.Subtract(q2);

            Assert.That(result.GetValue(), Is.EqualTo(5.0));
        }

        // subtraction negative operand
        [Test]
        public void testSubtraction_WithNegativeValues()
        {
            var q1 = new Quantity<LengthUnit>(5.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(-2.0, LengthUnit.Feet);

            var result = q1.Subtract(q2);

            Assert.That(result.GetValue(), Is.EqualTo(7.0));
        }

        // subtraction non commutative
        [Test]
        public void testSubtraction_NonCommutative()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(5.0, LengthUnit.Feet);

            var result1 = q1.Subtract(q2);
            var result2 = q2.Subtract(q1);

            Assert.That(result1.GetValue(), Is.EqualTo(5.0));
            Assert.That(result2.GetValue(), Is.EqualTo(-5.0));
        }

        // subtraction null operand
        [Test]
        public void testSubtraction_NullOperand()
        {
            var q = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);

            Assert.Throws<ArgumentException>(() => q.Subtract(null));
        }


        // division same unit
        [Test]
        public void testDivision_SameUnit_FeetDividedByFeet()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(2.0, LengthUnit.Feet);

            var result = q1.Divide(q2);

            Assert.That(result, Is.EqualTo(5.0));
        }

        // division same unit volume
        [Test]
        public void testDivision_SameUnit_LitreDividedByLitre()
        {
            var q1 = new Quantity<VolumeUnit>(10.0, VolumeUnit.Litre);
            var q2 = new Quantity<VolumeUnit>(5.0, VolumeUnit.Litre);

            var result = q1.Divide(q2);

            Assert.That(result, Is.EqualTo(2.0));
        }

        // division cross unit
        [Test]
        public void testDivision_CrossUnit_FeetDividedByInches()
        {
            var q1 = new Quantity<LengthUnit>(24.0, LengthUnit.Inches);
            var q2 = new Quantity<LengthUnit>(2.0, LengthUnit.Feet);

            var result = q1.Divide(q2);

            Assert.That(result, Is.EqualTo(1.0).Within(1e-4));
        }

        // division cross unit weight
        [Test]
        public void testDivision_CrossUnit_KilogramDividedByGram()
        {
            var q1 = new Quantity<WeightUnit>(2.0, WeightUnit.Kilogram);
            var q2 = new Quantity<WeightUnit>(2000.0, WeightUnit.Gram);

            var result = q1.Divide(q2);

            Assert.That(result, Is.EqualTo(1.0).Within(1e-4));
        }

        // division ratio greater than one
        [Test]
        public void testDivision_RatioGreaterThanOne()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(2.0, LengthUnit.Feet);

            var result = q1.Divide(q2);

            Assert.That(result, Is.GreaterThan(1.0));
        }

        // division ratio less than one
        [Test]
        public void testDivision_RatioLessThanOne()
        {
            var q1 = new Quantity<LengthUnit>(5.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);

            var result = q1.Divide(q2);

            Assert.That(result, Is.LessThan(1.0));
        }

        // division ratio equal one
        [Test]
        public void testDivision_RatioEqualToOne()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);

            var result = q1.Divide(q2);

            Assert.That(result, Is.EqualTo(1.0));
        }

        // division by zero
        [Test]
        public void testDivision_ByZero()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(0.0, LengthUnit.Feet);

            Assert.Throws<ArithmeticException>(() => q1.Divide(q2));
        }

        // division null operand
        [Test]
        public void testDivision_NullOperand()
        {
            var q = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);

            Assert.Throws<ArgumentException>(() => q.Divide(null));
        }

        // temperature equality tests

        [Test]
        public void testTemperatureEquality_CelsiusToCelsius_SameValue()
        {
            var t1 = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.Celsius);
            var t2 = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.Celsius);

            Assert.That(t1, Is.EqualTo(t2));
        }

        [Test]
        public void testTemperatureEquality_FahrenheitToFahrenheit_SameValue()
        {
            var t1 = new Quantity<TemperatureUnit>(32.0, TemperatureUnit.Fahrenheit);
            var t2 = new Quantity<TemperatureUnit>(32.0, TemperatureUnit.Fahrenheit);

            Assert.That(t1, Is.EqualTo(t2));
        }

        [Test]
        public void testTemperatureEquality_CelsiusToFahrenheit_Zero()
        {
            var t1 = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.Celsius);
            var t2 = new Quantity<TemperatureUnit>(32.0, TemperatureUnit.Fahrenheit);

            Assert.That(t1, Is.EqualTo(t2));
        }

        [Test]
        public void testTemperatureEquality_CelsiusToKelvin()
        {
            var t1 = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.Celsius);
            var t2 = new Quantity<TemperatureUnit>(273.15, TemperatureUnit.Kelvin);

            Assert.That(t1, Is.EqualTo(t2));
        }

        [Test]
        public void testTemperatureEquality_Negative40()
        {
            var t1 = new Quantity<TemperatureUnit>(-40.0, TemperatureUnit.Celsius);
            var t2 = new Quantity<TemperatureUnit>(-40.0, TemperatureUnit.Fahrenheit);

            Assert.That(t1, Is.EqualTo(t2));
        }

        [Test]
        public void testTemperatureEquality_SymmetricProperty()
        {
            var t1 = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.Celsius);
            var t2 = new Quantity<TemperatureUnit>(212.0, TemperatureUnit.Fahrenheit);

            Assert.That(t1.Equals(t2), Is.True);
            Assert.That(t2.Equals(t1), Is.True);
        }

        [Test]
        public void testTemperatureEquality_ReflexiveProperty()
        {
            var t = new Quantity<TemperatureUnit>(25.0, TemperatureUnit.Celsius);

            Assert.That(t.Equals(t), Is.True);
        }

        [Test]
        public void testTemperatureDifferentValuesInequality()
        {
            var t1 = new Quantity<TemperatureUnit>(50.0, TemperatureUnit.Celsius);
            var t2 = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.Celsius);

            Assert.That(t1.Equals(t2), Is.False);
        }

        [Test]
        public void testTemperatureConversion_CelsiusToFahrenheit()
        {
            var t = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.Celsius);

            var result = t.ConvertTo(TemperatureUnit.Fahrenheit);

            Assert.That(result.GetValue(), Is.EqualTo(212.0).Within(0.01));
        }

        [Test]
        public void testTemperatureConversion_FahrenheitToCelsius()
        {
            var t = new Quantity<TemperatureUnit>(32.0, TemperatureUnit.Fahrenheit);

            var result = t.ConvertTo(TemperatureUnit.Celsius);

            Assert.That(result.GetValue(), Is.EqualTo(0.0).Within(0.01));
        }

        [Test]
        public void testTemperatureConversion_CelsiusToKelvin()
        {
            var t = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.Celsius);

            var result = t.ConvertTo(TemperatureUnit.Kelvin);

            Assert.That(result.GetValue(), Is.EqualTo(273.15).Within(0.01));
        }

        [Test]
        public void testTemperatureConversion_KelvinToCelsius()
        {
            var t = new Quantity<TemperatureUnit>(273.15, TemperatureUnit.Kelvin);

            var result = t.ConvertTo(TemperatureUnit.Celsius);

            Assert.That(result.GetValue(), Is.EqualTo(0.0).Within(0.01));
        }

        [Test]
        public void testTemperatureConversion_NegativeValues()
        {
            var t = new Quantity<TemperatureUnit>(-20.0, TemperatureUnit.Celsius);

            var result = t.ConvertTo(TemperatureUnit.Fahrenheit);

            Assert.That(result.GetValue(), Is.EqualTo(-4.0).Within(0.01));
        }

        [Test]
        public void testTemperatureConversion_RoundTrip()
        {
            var t = new Quantity<TemperatureUnit>(50.0, TemperatureUnit.Celsius);

            var f = t.ConvertTo(TemperatureUnit.Fahrenheit);
            var c = f.ConvertTo(TemperatureUnit.Celsius);

            Assert.That(c.GetValue(), Is.EqualTo(50.0).Within(0.01));
        }
        // ENTITY TESTS 

        [Test]
        public void testQuantityEntity_StoresValueAndUnit()
        {
            var q = new Quantity<LengthUnit>(5.0, LengthUnit.Feet);

            Assert.That(q.GetValue(), Is.EqualTo(5.0));
            Assert.That(q.GetUnit(), Is.EqualTo(LengthUnit.Feet));
        }

        [Test]
        public void testQuantityEntity_InvalidValueThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
                new Quantity<LengthUnit>(double.NaN, LengthUnit.Feet));
        }

        [Test]
        public void testQuantityEntity_EqualsWorksAcrossUnits()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Yards);
            var q2 = new Quantity<LengthUnit>(3.0, LengthUnit.Feet);

            Assert.That(q1.Equals(q2), Is.True);
        }


        // SERVICE LOGIC TESTS 

        [Test]
        public void testService_Add_LengthUnits()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12.0, LengthUnit.Inches);

            var result = q1.Add(q2);

            Assert.That(result.GetValue(), Is.EqualTo(2.0));
        }

        [Test]
        public void testService_Subtract_LengthUnits()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(6.0, LengthUnit.Inches);

            var result = q1.Subtract(q2);

            Assert.That(result.GetValue(), Is.EqualTo(9.5).Within(0.001));
        }

        [Test]
        public void testService_Divide_LengthUnits()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(2.0, LengthUnit.Feet);

            var result = q1.Divide(q2);

            Assert.That(result, Is.EqualTo(5.0));
        }


        // ERROR HANDLING 

        [Test]
        public void testService_DivideByZero_ThrowsException()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(0.0, LengthUnit.Feet);

            Assert.Throws<ArithmeticException>(() => q1.Divide(q2));
        }

        [Test]
        public void testService_NullOperandRejected()
        {
            var q = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);

            Assert.Throws<ArgumentException>(() => q.Subtract(null));
        }


        // CONVERSION 

        [Test]
        public void testService_Convert_Length()
        {
            var q = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);

            var result = q.ConvertTo(LengthUnit.Inches);

            Assert.That(result.GetValue(), Is.EqualTo(12.0));
        }

        [Test]
        public void testService_Convert_Volume()
        {
            var q = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);

            var result = q.ConvertTo(VolumeUnit.Millilitre);

            Assert.That(result.GetValue(), Is.EqualTo(1000.0));
        }


        // TEMPERATURE SUPPORT

        [Test]
        public void testService_TemperatureEquality()
        {
            var t1 = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.Celsius);
            var t2 = new Quantity<TemperatureUnit>(32.0, TemperatureUnit.Fahrenheit);

            Assert.That(t1.Equals(t2), Is.True);
        }

        [Test]
        public void testService_TemperatureConversion()
        {
            var t = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.Celsius);

            var result = t.ConvertTo(TemperatureUnit.Kelvin);

            Assert.That(result.GetValue(), Is.EqualTo(273.15).Within(0.01));
        }


        // CROSS CATEGORY SAFETY

        [Test]
        public void testCrossCategoryComparison_ReturnsFalse()
        {
            var volume = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var length = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);

            Assert.That(volume.Equals(length), Is.False);
        }




    }
}