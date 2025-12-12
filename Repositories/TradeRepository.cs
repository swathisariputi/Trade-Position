using System.Collections.Concurrent;
using Trade_Position.Interfaces;
using Trade_Position.Models;

namespace Trade_Position.Repositories
{
    public class TradeRepository : ITradeRepository
    {
        // instrument -> thread-safe list (we store as ConcurrentQueue for append-only semantics)
        private readonly string _connectionString;
        public TradeRepository() 
        {
            _connectionString = "";
        }

        public void AddToTradeHistory(Trade trade)
        {
        }

        public IReadOnlyCollection<Trade> GetAllTrade()
        {
            var list = new List<Trade>();
            return list;
        }

        public IReadOnlyCollection<Trade> GetByAsset(string Asset)
        {
            return Array.Empty<Trade>();
        }

        public void AddToPositionHistory(Position position)
        {
        }

        public IReadOnlyCollection<Position> GetAllPostion()
        {
            var list = new List<Position>();
            return list;
        }

        public Position GetPositionByAsset(string Asset)
        {
            Position postion = new();
            return postion;
        }

    }
}
