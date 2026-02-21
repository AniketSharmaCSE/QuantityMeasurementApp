using NUnit.Framework;
using QuantityMeasurementApp;

namespace QuantityMeasurementAppTests
{
    [TestFixture]
    public class QuantityMeasurementAppTest
    {
        [Test]
        public void TestEquality_SameValue()
        {
            Feet feet1 = new Feet(1.0);
            Feet feet2 = new Feet(1.0);

            bool result = feet1.Equals(feet2);

            NUnit.Framework.Assert.That(result, Is.True);
        }

        [Test]
        public void TestEquality_DifferentValue()
        {
            Feet feet1 = new Feet(1.0);
            Feet feet2 = new Feet(2.0);

            bool result = feet1.Equals(feet2);

            NUnit.Framework.Assert.That(result, Is.False);
        }

        [Test]
        public void TestEquality_NullComparison()
        {
            Feet feet = new Feet(1.0);

            bool result = feet.Equals(null);

            NUnit.Framework.Assert.That(result, Is.False);
        }

        [Test]
        public void TestEquality_NonNumericInput()
        {
            Feet feet = new Feet(1.0);
            object other = new object();

            bool result = feet.Equals(other);

            NUnit.Framework.Assert.That(result, Is.False);
        }

        [Test]
        public void TestEquality_SameReference()
        {
            Feet feet = new Feet(1.0);

            bool result = feet.Equals(feet);

            NUnit.Framework.Assert.That(result, Is.True);
        }
    }
}