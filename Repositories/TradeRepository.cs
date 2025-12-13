using Microsoft.Data.SqlClient;
using System.Diagnostics;
using Trade_Position.Constants;
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
            _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=Trade;Trusted_Connection=True;";
        }

        public decimal AddToTradeHistory(Trade trade)
        {
            try
            {
                using SqlConnection conn = new(_connectionString);
                using SqlCommand cmd = new(DBContants.SP_add_trade_history, conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue(DBContants.Account, trade.Account);
                cmd.Parameters.AddWithValue(DBContants.Asset, trade.Asset);
                cmd.Parameters.AddWithValue(DBContants.Price, trade.Price);
                cmd.Parameters.AddWithValue(DBContants.TradeType, trade.TradeType);
                cmd.Parameters.AddWithValue(DBContants.Quantity, trade.Quantity);
                conn.Open();
                return Convert.ToDecimal(cmd.ExecuteScalar());
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error while adding trade: ", ex.ToString());
                throw;
            }
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
            try
            {
                using SqlConnection conn = new(_connectionString);
                using SqlCommand cmd = new(DBContants.SP_add_update_position, conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue(DBContants.Account, position.Account);
                cmd.Parameters.AddWithValue(DBContants.Asset, position.Asset);
                cmd.Parameters.AddWithValue(DBContants.NetQuantity, position.NetQuantity);
                cmd.Parameters.AddWithValue(DBContants.AveragePrice, position.AveragePrice);
                cmd.Parameters.AddWithValue(DBContants.RealizedPnl, position.RealizedPnl);
                cmd.Parameters.AddWithValue(DBContants.NotionalValue, position.NotionalValue);
                cmd.Parameters.AddWithValue(DBContants.PositionStatus, position.PositionStatus);
                conn.Open();
                cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while adding position: ", ex.ToString());
                throw;
            }
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
