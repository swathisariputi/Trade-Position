using Trade_Position.Constants;
using Trade_Position.Models;

namespace Trade_Position.Interfaces
{
    public interface ITradeService
    {
        /// <summary>
        /// Adds the trade and calulated position 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public string SubmitTradeDetails(Trade t, string action);

        /// <summary>
        /// Retrieve all the Trades
        /// </summary>
        /// <returns></returns>
        public IReadOnlyCollection<Trade> ListTrades();

        /// <summary>
        /// Retrieve all the positions (all assets) for each account 
        /// </summary>
        /// <returns></returns>
        public IReadOnlyCollection<Position> GetAllPositions();

        /// <summary>
        /// Get Position of an asset for an account
        /// </summary>
        /// <param name="Asset"></param>
        /// <returns></returns>
        public Position GetPosition(string Account, string Asset);
    }
}
