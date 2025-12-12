using System.ComponentModel.DataAnnotations;

namespace Trade_Position.Models
{
    public sealed class Position
    {
        public int PositionId { get; set; }
        public string Account { get; set; }
        public string Asset { get; init; } = default!;
        public long NetQuantity { get; init; }
        public decimal? AveragePrice { get; init; } 
        public decimal RealizedPnl { get; init; } 

        public decimal NotionalValue { get; set; }

        public string PositionStatus { get; set; } = default!;

        public DateTime LastUpdated { get; set; }
    }
}
