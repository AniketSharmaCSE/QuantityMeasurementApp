using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    public class EqualityChecker
    {
        public bool CheckEquality(Quantity q1, Quantity q2)
        {
            if (q1 == null || q2 == null)
            {
                return false;
            }

            return q1.Equals(q2);
        }
    }
}