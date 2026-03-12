using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    public class EqualityChecker
    {
        public bool CheckEquality<U>(Quantity<U> q1, Quantity<U> q2)
        {
            if (q1 == null || q2 == null)
            {
                return false;
            }

            return q1.Equals(q2);
        }
    }
}