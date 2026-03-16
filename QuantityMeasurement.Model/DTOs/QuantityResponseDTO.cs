using QuantityMeasurement.Model.DTOs;

namespace QuantityMeasurement.Model.DTOs
{
    public class QuantityResponseDTO
    {
        public bool Success { get; set; }

        public string Operation { get; set; } = string.Empty;

        public QuantityDTO? Operand1 { get; set; }

        public QuantityDTO? Operand2 { get; set; }

        public QuantityDTO? Result { get; set; }

        public bool? BoolResult { get; set; }

        public double? ScalarResult { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;

        public static QuantityResponseDTO ForEquality(QuantityDTO a, QuantityDTO b, bool equal)
        {
            return new QuantityResponseDTO
            {
                Success = true,
                Operation = "Compare",
                Operand1 = a,
                Operand2 = b,
                BoolResult = equal
            };
        }

        public static QuantityResponseDTO ForConversion(QuantityDTO input, QuantityDTO output)
        {
            return new QuantityResponseDTO
            {
                Success = true,
                Operation = "Convert",
                Operand1 = input,
                Result = output
            };
        }

        public static QuantityResponseDTO ForArithmetic(string op, QuantityDTO a, QuantityDTO b, QuantityDTO result)
        {
            return new QuantityResponseDTO
            {
                Success = true,
                Operation = op,
                Operand1 = a,
                Operand2 = b,
                Result = result
            };
        }

        public static QuantityResponseDTO ForDivision(QuantityDTO a, QuantityDTO b, double scalar)
        {
            return new QuantityResponseDTO
            {
                Success = true,
                Operation = "Divide",
                Operand1 = a,
                Operand2 = b,
                ScalarResult = scalar
            };
        }

        public static QuantityResponseDTO ForError(string operation, string message)
        {
            return new QuantityResponseDTO
            {
                Success = false,
                Operation = operation,
                ErrorMessage = message
            };
        }

        public override string ToString()
        {
            if (!Success)
            {
                return $"Error during {Operation}: {ErrorMessage}";
            }

            if (Operation == "Compare")
            {
                return $"[Compare] {Operand1} == {Operand2} -> {BoolResult}";
            }

            if (Operation == "Convert")
            {
                return $"[Convert] {Operand1} -> {Result}";
            }

            if (Operation == "Divide")
            {
                return $"[Divide] {Operand1} / {Operand2} = {ScalarResult:F4}";
            }

            return $"[{Operation}] {Operand1} {Operation} {Operand2} = {Result}";
        }
    }
}