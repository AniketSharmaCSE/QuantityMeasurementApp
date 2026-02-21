namespace QuantityMeasurementApp;

public class Feet
{
    private readonly double feetValue;

    public Feet(double value)
    {
        feetValue = value;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
            return true;

        if (obj == null)
            return false;

        if (GetType() != obj.GetType())
            return false;

        Feet other = (Feet)obj;
        return double.Equals(feetValue, other.feetValue);
    }

    public override int GetHashCode()
    {
        return feetValue.GetHashCode();
    }
}
