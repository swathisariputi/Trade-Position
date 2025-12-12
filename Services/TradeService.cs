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

        public void AddTrade(Trade t) => _repo.AddToTradeHistory(t);

        public IReadOnlyCollection<Trade> ListTrades() => _repo.GetAllTrade();

        public IReadOnlyCollection<Position> GetAllPositions() => _repo.GetAllPostion();

        public Position GetPosition(string Asset) => _repo.GetPositionByAsset(Asset);

    }
}
