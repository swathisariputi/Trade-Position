using System.ComponentModel.DataAnnotations;

namespace Trade_Position.Models
{
    /// <summary>
    /// Model for a trade
    /// </summary>
    public sealed class Trade
    {

        public int TradeId { get; init; }

        [Required]
        public string Asset { get; set; } = default!;

        [Required]
        public string Account { get; set; }

        [Required]
        public TradeType TradeType { get; set; }

        [Required]
        [Range(1, long.MaxValue)]
        public long Quantity { get; set; }

        [Required]
        [Range(typeof(decimal), "0.00000001", "79228162514264337593543950335")]
        public decimal Price { get; set; }

        public DateTime TradeTimeStamp { get; set; }
    }

    public enum TradeType
    {
        BUY,
        SELL
    }
}
