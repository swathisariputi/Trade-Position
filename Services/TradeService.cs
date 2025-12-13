using Trade_Position.Interfaces;
using Trade_Position.Models;

namespace Trade_Position.Services
{
    /// <summary>
    /// Service to write logic for endpoints
    /// </summary>
    public class TradeService
    {
        private readonly ITradeRepository _repo;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="repo"></param>
        public TradeService(ITradeRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Adds the trade and calulated position 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public decimal AddTrade(Trade t) {
            try
            {
                Position pos = CalculatePosition(t); 
                _repo.AddOrUpdatePosition(pos); 
                var tradeId = _repo.AddToTradeHistory(t);
                return tradeId;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
         }

        /// <summary>
        /// Retrieve all the Trades
        /// </summary>
        /// <returns></returns>
        public IReadOnlyCollection<Trade> ListTrades() => _repo.GetAllTrade();

        /// <summary>
        /// Retrieve all the positions (all assets) for each account 
        /// </summary>
        /// <returns></returns>
        public IReadOnlyCollection<Position> GetAllPositions() => _repo.GetAllPostion();

        /// <summary>
        /// Get Position of an asset for an account
        /// </summary>
        /// <param name="Asset"></param>
        /// <returns></returns>
        public Position GetPosition(string Asset) => _repo.GetPositionByAsset(Asset);

        /// <summary>
        /// Caluculates the position of an asset in an account when trade is added
        /// </summary>
        /// <param name="trade"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private Position CalculatePosition(Trade trade)
        {
            var position = new Position()
            {
                Account = trade.Account,
                Asset = trade.Asset,
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
