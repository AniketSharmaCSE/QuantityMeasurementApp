using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuantityMeasurement.Model.Entities.EF
{
    [Table("quantity_measurements_ef")]
    public class MeasurementRecord
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(50)]
        public string Operation { get; set; } = string.Empty;

        public double? Operand1Value { get; set; }
        public string? Operand1Unit { get; set; }
        public string? Operand1Category { get; set; }

        public double? Operand2Value { get; set; }
        public string? Operand2Unit { get; set; }
        public string? Operand2Category { get; set; }

        public double? ResultValue { get; set; }
        public string? ResultUnit { get; set; }
        public string? ResultCategory { get; set; }

        public bool? BoolResult { get; set; }
        public double? ScalarResult { get; set; }

        public bool HasError { get; set; }
        public string? ErrorMessage { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
