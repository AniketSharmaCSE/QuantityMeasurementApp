namespace QuantityMeasurementApp;

public class EqualityChecker
{
    public bool AreEqual(Feet first, Feet second)
    {
        if (first == null)
            return false;
        if (second == null)
            return false;

        return first.Equals(second);
    }
}
