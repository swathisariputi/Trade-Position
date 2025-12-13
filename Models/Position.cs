using System.ComponentModel.DataAnnotations;

namespace Trade_Position.Models
{
    /// <summary>
    /// Model for Position
    /// </summary>
    public sealed class Position
    {
        public int PositionId { get; set; }
        public string Account { get; set; }
        public string Asset { get; set; } = default!;
        public long NetQuantity { get; set; } = 0;
        public decimal AveragePrice { get; set; } = 0;
        public decimal RealizedPnl { get; set; } = 0;

        public decimal NotionalValue { get; set; } = 0;

        public string PositionStatus { get; set; } = "OPEN";

        public DateTime LastUpdated { get; set; }
    }
}
