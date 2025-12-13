using Trade_Position.Constants;
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
        public string SubmitTradeDetails(Trade t, string action) {
            try
            {
                if (action == DBContants.action_add)
                {
                    Position pos = CalculatePosition(t);
                    _repo.AddOrUpdatePosition(pos);
                    var tradeId = _repo.AddOrUpdateTrade(t);
                    return $"Succefully added the trade details with tradeId: {tradeId}";
                }
                else
                {
                    //Update the trade and recalculate positions for old trade and updated trade related details
                    Trade oldTrad = _repo.GetTradeByTradeId(t.TradeId);
                    var tradeId = _repo.AddOrUpdateTrade(t);
                    var pos = ReCalculatePosition(oldTrad.Account, oldTrad.Asset);
                    var pos_new =ReCalculatePosition(t.Account, t.Asset);
                    _repo.AddOrUpdatePosition(pos_new);
                    _repo.AddOrUpdatePosition(pos);
                    return $"Succefully updated the trade details with tradeId: {t.TradeId}";
                }
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
        public Position GetPosition(string Account, string Asset) => _repo.GetPositionOfAssetInAccount(Account, Asset);

        /// <summary>
        /// Caluculates the position of an asset in an account when trade is added
        /// </summary>
        /// <param name="trade"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private Position CalculatePosition(Trade trade)
        {
            var position = GetPosition(trade.Account, trade.Asset);  
            if (position==null){
                position = new Position()
                {
                    Account = trade.Account,
                    Asset = trade.Asset
                };
            }

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

        /// <summary>
        /// To recalculate the position
        /// </summary>
        /// <param name="account"></param>
        /// <param name="asset"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private Position ReCalculatePosition(string account, string asset)
        {
            var position = new Position
            {
                Account = account,
                Asset = asset
            };
            var allTrades = _repo.GetAllTrade();
            // Filter relevant trades
            var trades = allTrades
                .Where(t =>
                    t.Account == account &&
                    t.Asset == asset)
                .OrderBy(t => t.TradeTimeStamp)
                .ToList();
            if(trades.Count == 0)
            {
                position.PositionStatus = "Cancelled";
                return position;
            }
            foreach (var trade in trades)
            {
                if (trade.TradeType == TradeType.BUY.ToString())
                {
                    var totalCost = (position.NetQuantity * position.AveragePrice) +  (trade.Quantity * trade.Price);
                    position.NetQuantity += trade.Quantity;
                    position.AveragePrice = position.NetQuantity == 0 ? 0 : totalCost / position.NetQuantity;
                }
                else if (trade.TradeType == TradeType.SELL.ToString())
                {
                    if (trade.Quantity > position.NetQuantity)
                        throw new InvalidOperationException(
                            $"Sell quantity {trade.Quantity} exceeds position {position.NetQuantity}");

                    var realizedPnl = trade.Quantity * (trade.Price - position.AveragePrice);
                    position.RealizedPnl += realizedPnl;
                    position.NetQuantity -= trade.Quantity;
                }
            }
            position.NotionalValue = Math.Abs(position.NetQuantity * position.AveragePrice);
            position.PositionStatus = position.NetQuantity == 0 ? "CLOSED" : "OPEN";
            return position;
        }
    }
}
