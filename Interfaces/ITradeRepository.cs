using Trade_Position.Models;

namespace Trade_Position.Interfaces
{
    public interface ITradeRepository
    {
        public void AddToTradeHistory(Trade trade);

        public IReadOnlyCollection<Trade> GetAllTrade();

        public IReadOnlyCollection<Trade> GetByAsset(string Asset);

        public void AddToPositionHistory(Position position);
        public IReadOnlyCollection<Position> GetAllPostion();

        public Position GetPositionByAsset(string Asset);
    }
}
