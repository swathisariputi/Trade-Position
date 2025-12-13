using Trade_Position.Models;

namespace Trade_Position.Interfaces
{
    public interface ITradeRepository
    {
        public decimal AddOrUpdateTrade(Trade trade);

        public IReadOnlyCollection<Trade> GetAllTrade();

        public void AddOrUpdatePosition(Position position);
        public IReadOnlyCollection<Position> GetAllPostion();

        public Position GetPositionOfAssetInAccount(string Account, string Asset);

        public Trade GetTradeByTradeId(int TradeId);
    }
}
