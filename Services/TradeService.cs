using System.Diagnostics;
using Trade_Position.Interfaces;
using Trade_Position.Models;

namespace Trade_Position.Services
{
    public class TradeService
    {
        private readonly ITradeRepository _repo;

        public TradeService(ITradeRepository repo)
        {
            _repo = repo;
        }

        public decimal AddTrade(Trade t) {
            try
            {
                Position pos = CalculatePosition(t);
                _repo.AddToPositionHistory(pos);
                var tradeId = _repo.AddToTradeHistory(t);
                return tradeId;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
         }

        public IReadOnlyCollection<Trade> ListTrades() => _repo.GetAllTrade();

        public IReadOnlyCollection<Position> GetAllPositions() => _repo.GetAllPostion();

        public Position GetPosition(string Asset) => _repo.GetPositionByAsset(Asset);

        public Position CalculatePosition(Trade trade)
        {
            var position = new Position()
            {
                Account = trade.Account,
                Asset = trade.Asset,
                AveragePrice=0,
                
            };

            if (trade.TradeType == TradeType.BUY.ToString())
            {
                var totalCost = (position.NetQuantity * position.AveragePrice) + (trade.Quantity * trade.Price);

                position.NetQuantity += trade.Quantity;

                position.AveragePrice = position.NetQuantity == 0 ? 0 : totalCost / position.NetQuantity;
            }
            else if (trade.TradeType == TradeType.SELL.ToString())
            {
                if (trade.Quantity > position.NetQuantity)
                    throw new InvalidOperationException("Sell quantity exceeds available position.");

                position.RealizedPnl += trade.Quantity * (trade.Price - position.AveragePrice);
                position.NetQuantity -= trade.Quantity;
                if (position.NetQuantity == 0)
                    position.AveragePrice = 0;
            }

            position.NotionalValue = Math.Abs(position.NetQuantity * position.AveragePrice);
            position.PositionStatus = position.NetQuantity == 0 ? "CLOSED" : "OPEN";
            return position;
        }

    }
}
