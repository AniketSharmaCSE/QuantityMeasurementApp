namespace QuantityMeasurement.Model.DTOs
{
    // returned by every service method back to the controller
    public class QuantityResponseDTO
    {
        public bool Success { get; set; }
        public string Operation { get; set; } = string.Empty;

        public QuantityDTO? Operand1 { get; set; }
        public QuantityDTO? Operand2 { get; set; }
        public QuantityDTO? Result { get; set; }

        // compare operations set this
        public bool? BoolResult { get; set; }

        // divide sets this instead of Result
        public double? ScalarResult { get; set; }

        public string? ErrorMessage { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public static QuantityResponseDTO ForArithmetic(
            string operation,
            QuantityDTO operand1,
            QuantityDTO operand2,
            QuantityDTO result)
        {
            return new QuantityResponseDTO
            {
                Success   = true,
                Operation = operation,
                Operand1  = operand1,
                Operand2  = operand2,
                Result    = result
            };
        }

        public static QuantityResponseDTO ForConversion(
            QuantityDTO source,
            QuantityDTO converted)
        {
            return new QuantityResponseDTO
            {
                Success   = true,
                Operation = "Convert",
                Operand1  = source,
                Result    = converted
            };
        }

        public static QuantityResponseDTO ForComparison(
            string operation,
            QuantityDTO operand1,
            QuantityDTO operand2,
            bool isEqual)
        {
            return new QuantityResponseDTO
            {
                Success    = true,
                Operation  = operation,
                Operand1   = operand1,
                Operand2   = operand2,
                BoolResult = isEqual,
                Result     = new QuantityDTO(0, isEqual ? "Equal" : "NotEqual", "Comparison")
            };
        }

        public static QuantityResponseDTO ForDivision(
            string operation,
            QuantityDTO operand1,
            QuantityDTO operand2,
            double scalar)
        {
            return new QuantityResponseDTO
            {
                Success      = true,
                Operation    = operation,
                Operand1     = operand1,
                Operand2     = operand2,
                ScalarResult = scalar,
                Result       = new QuantityDTO(scalar, "Scalar", "Division")
            };
        }

        public static QuantityResponseDTO ForError(string operation, string message)
        {
            return new QuantityResponseDTO
            {
                Success      = false,
                Operation    = operation,
                ErrorMessage = message
            };
        }

        public override string ToString()
        {
            if (!Success)
                return $"[ERROR] {Operation}: {ErrorMessage}";

            if (BoolResult.HasValue)
                return $"{Operation}: {Operand1} == {Operand2} => {BoolResult}";

            if (ScalarResult.HasValue)
                return $"{Operation}: {Operand1} / {Operand2} => {ScalarResult}";

            return $"{Operation}: {Operand1} -> {Result}";
        }
    }
}
