using System;

namespace QuantityMeasurement.Repository.Exception
{
    public class DatabaseException : System.Exception
    {
        // which repo method failed, e.g. "Save" or "GetAll"
        public string Operation { get; }

        public DatabaseException(string operation, string message)
            : base($"[DB:{operation}] {message}")
        {
            Operation = operation;
        }

        // overload when wrapping a caught SqlException so the original stack trace is kept
        public DatabaseException(string operation, string message, System.Exception inner)
            : base($"[DB:{operation}] {message}", inner)
        {
            Operation = operation;
        }
    }
}
