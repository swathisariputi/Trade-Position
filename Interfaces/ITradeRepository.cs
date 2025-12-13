using Trade_Position.Models;

namespace Trade_Position.Interfaces
{
    public interface ITradeRepository
    {
        public decimal AddToTradeHistory(Trade trade);

        public IReadOnlyCollection<Trade> GetAllTrade();

        public void AddToPositionHistory(Position position);
        public IReadOnlyCollection<Position> GetAllPostion();

        public Position GetPositionByAsset(string Asset);
    }
}
