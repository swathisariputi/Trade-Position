using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using Trade_Position.Constants;
using Trade_Position.Interfaces;
using Trade_Position.Models;

namespace Trade_Position.Repositories
{
    /// <summary>
    /// To connect to Trade database and perform sql commands
    /// </summary>
    public class TradeRepository : ITradeRepository
    {
        // connection string to connect to db - since its local db not maintained it securely
        // can be maintained as encrypted password in app setting and build connection string for making it more secure
        private readonly string _connectionString;
        public TradeRepository() 
        {
            _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=Trade;Trusted_Connection=True;";
        }

        /// <summary>
        /// Adds Trade to the TradeHistory table
        /// </summary>
        /// <param name="trade"></param>
        /// <returns></returns>
        public decimal AddOrUpdateTrade(Trade trade)
        {
            try
            {
                using SqlConnection conn = new(_connectionString);
                using SqlCommand cmd = new(DBContants.SP_add_or_update_trade, conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue(DBContants.TradeId, trade.TradeId);
                cmd.Parameters.AddWithValue(DBContants.Account, trade.Account);
                cmd.Parameters.AddWithValue(DBContants.Asset, trade.Asset);
                cmd.Parameters.AddWithValue(DBContants.Price, trade.Price);
                cmd.Parameters.AddWithValue(DBContants.TradeType, trade.TradeType);
                cmd.Parameters.AddWithValue(DBContants.Quantity, trade.Quantity);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error while adding trade: ", ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// returns all the trades 
        /// </summary>
        /// <returns></returns>
        public IReadOnlyCollection<Trade> GetAllTrade()
        {
            try {
                DataSet dataset = new();
                var list = new List<Trade>();
                using SqlConnection conn = new(_connectionString);
                using SqlCommand cmd = new(DBContants.SP_get_all_trades, conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
                conn.Open();
                sqlDataAdapter.Fill(dataset);
                if (dataset.Tables[0].Rows.Count > 0)
                {
                    list = (from DataRow dr in dataset.Tables[0].Rows
                            select new Trade()
                            {
                                Account = Convert.ToString(dr[DBContants.Account]),
                                Asset = Convert.ToString(dr[DBContants.Asset]),
                                Price = Convert.ToDecimal(dr[DBContants.Price]),
                                Quantity = Convert.ToInt64(dr[DBContants.Quantity]),
                                TradeId = Convert.ToInt32(dr[DBContants.TradeId]),
                                TradeType = Convert.ToString(dr[DBContants.TradeType]),
                                TradeTimeStamp = Convert.ToDateTime(dr[DBContants.TradeTimeStamp])
                            }).ToList();
                }

                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while fetching trades: ", ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Updates the position if the position is already added to a asset for an account else adds position
        /// </summary>
        /// <param name="position"></param>
        public void AddOrUpdatePosition(Position position)
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

        /// <summary>
        /// Returns the positions of all assets for all accounts
        /// </summary>
        /// <returns></returns>
        public IReadOnlyCollection<Position> GetAllPostion()
        {
            try
            {
                DataSet dataset = new();
                var list = new List<Position>();
                using SqlConnection conn = new(_connectionString);
                using SqlCommand cmd = new(DBContants.SP_get_all_positions, conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
                conn.Open();
                sqlDataAdapter.Fill(dataset);
                if (dataset.Tables[0].Rows.Count > 0)
                {
                    list = (from DataRow dr in dataset.Tables[0].Rows
                            select new Position()
                            {
                                Account = Convert.ToString(dr[DBContants.Account]),
                                Asset = Convert.ToString(dr[DBContants.Asset]),
                                AveragePrice = Convert.ToDecimal(dr[DBContants.AveragePrice]),
                                NetQuantity = Convert.ToInt64(dr[DBContants.NetQuantity]),
                                PositionId = Convert.ToInt32(dr[DBContants.PositionId]),
                                PositionStatus = Convert.ToString(dr[DBContants.PositionStatus]),
                                LastUpdated = Convert.ToDateTime(dr[DBContants.LastUpdated]),
                                NotionalValue = Convert.ToDecimal(dr[DBContants.NotionalValue]),
                                RealizedPnl = Convert.ToDecimal(dr[DBContants.RealizedPnl])
                            }).ToList();
                }

                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while fetching positions: ", ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Returns Position of an asset in an account
        /// </summary>
        /// <param name="Asset"></param>
        /// <returns></returns>
        public Position GetPositionOfAssetInAccount(string Account, string Asset)
        {
            Position position = new();
            try
            {
                DataSet dataset = new();
                using SqlConnection conn = new(_connectionString);
                using SqlCommand cmd = new(DBContants.SP_get_postion_of_asset_in_account, conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue(DBContants.Account, Account);
                cmd.Parameters.AddWithValue(DBContants.Asset, Asset);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
                conn.Open();
                sqlDataAdapter.Fill(dataset);
                if (dataset.Tables[0].Rows.Count > 0)
                {
                    position.Account = Convert.ToString(dataset.Tables[0].Rows[0][DBContants.Account]);
                    position.Asset = Convert.ToString(dataset.Tables[0].Rows[0][DBContants.Asset]);
                    position.AveragePrice = Convert.ToDecimal(dataset.Tables[0].Rows[0][DBContants.AveragePrice]);
                    position.NetQuantity = Convert.ToInt64(dataset.Tables[0].Rows[0][DBContants.NetQuantity]);
                    position.PositionId = Convert.ToInt32(dataset.Tables[0].Rows[0][DBContants.PositionId]);
                    position.PositionStatus = Convert.ToString(dataset.Tables[0].Rows[0][DBContants.PositionStatus]);
                    position.LastUpdated = Convert.ToDateTime(dataset.Tables[0].Rows[0][DBContants.LastUpdated]);
                    position.NotionalValue = Convert.ToDecimal(dataset.Tables[0].Rows[0][DBContants.NotionalValue]);
                    position.RealizedPnl = Convert.ToDecimal(dataset.Tables[0].Rows[0][DBContants.RealizedPnl]);
                    return position;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while fetching position of a asset: ", ex.ToString());
                throw;
            }
        }

        public Trade GetTradeByTradeId(int TradeId)
        {
            Trade trade = null;
            try
            {
                DataSet dataset = new();
                using SqlConnection conn = new(_connectionString);
                using SqlCommand cmd = new(DBContants.SP_get_trade_by_tradeId, conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue(DBContants.TradeId, TradeId);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
                conn.Open();
                sqlDataAdapter.Fill(dataset);
                if (dataset.Tables[0].Rows.Count > 0)
                {
                    trade = new Trade()
                    {
                        Account = Convert.ToString(dataset.Tables[0].Rows[0][DBContants.Account]),
                        Asset = Convert.ToString(dataset.Tables[0].Rows[0][DBContants.Asset]),
                        Price = Convert.ToDecimal(dataset.Tables[0].Rows[0][DBContants.Price]),
                        Quantity = Convert.ToInt64(dataset.Tables[0].Rows[0][DBContants.Quantity]),
                        TradeId = Convert.ToInt32(dataset.Tables[0].Rows[0][DBContants.TradeId]),
                        TradeType = Convert.ToString(dataset.Tables[0].Rows[0][DBContants.TradeType]),
                        TradeTimeStamp = Convert.ToDateTime(dataset.Tables[0].Rows[0][DBContants.TradeTimeStamp])
                    };
                }
                return trade;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while fetching position of a asset: ", ex.ToString());
                throw;
            }
        }
    }
}
