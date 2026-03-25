namespace QuantityMeasurement.Api.Models
{
    // request body for two-operand operations (compare, calculate)
    // Category: Length, Weight, Volume, Temperature
    // Operation (calculate only): Add, Subtract, Divide
    public class QuantityRequest
    {
        public double Value1 { get; set; }
        public string Unit1 { get; set; } = string.Empty;
        public double Value2 { get; set; }
        public string Unit2 { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string? Operation { get; set; }
        public string? TargetUnit { get; set; }
    }

    // request body for single-operand convert
    // Category: Length, Weight, Volume, Temperature
    public class ConvertRequest
    {
        public double Value { get; set; }
        public string FromUnit { get; set; } = string.Empty;
        public string ToUnit { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
    }
}
