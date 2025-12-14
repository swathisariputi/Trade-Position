using FluentValidation;
using Trade_Position.Models;

namespace Trade_Position.Validators
{
    public class TradeValidator: AbstractValidator<Trade>
    {
        public TradeValidator()
        {
            RuleFor(t => t.Account).NotEmpty().WithMessage("Account is required").MaximumLength(50);

            RuleFor(t => t.Asset).NotEmpty().WithMessage("Asset is required").MaximumLength(50);

            RuleFor(t => t.Price).GreaterThan(0).WithMessage("Price must be greater than zero");

            RuleFor(t => t.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero");

            RuleFor(t => t.TradeType).Must(BeValidTradeType).WithMessage("TradeType must be 0 or 1 indicating BUY or SELL");
        }
        private bool BeValidTradeType(TradeType tradeType)
        {
            return tradeType == TradeType.BUY || tradeType == TradeType.SELL;
        }
    }
}
