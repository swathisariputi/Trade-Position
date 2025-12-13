using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Trade_Position.Constants;
using Trade_Position.Models;
using Trade_Position.Services;

namespace Trade_Position.Controllers
{
    /// <summary>
    /// Trade Controller that exposes REST endpoints for adding trades and retrieving trades and positions
    /// </summary>
    [ApiVersion(API_VERSION_01)]
    [Route(Routing)]
    [ApiController]
    public class TradeController : ControllerBase
    {
        private const string API_VERSION_01 = "1";
        private const string Routing = "api/V{version:apiVersion}";
        private readonly TradeService _tradeService;

        /// <summary>
        /// Constructor initiates TradeService
        /// </summary>
        /// <param name="svc"></param>
        public TradeController(TradeService svc) => _tradeService = svc;

        /// <summary>
        /// Adds Trade using rest service
        /// </summary>
        /// <param name="trade"></param>
        /// <returns></returns>
        [HttpPost("add/trade")]
        public ActionResult<Trade> AddTrade([FromBody] Trade trade)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = _tradeService.SubmitTradeDetails(trade, DBContants.action_add);
            return Ok(response);
        }

        /// <summary>
        /// updates Trade
        /// </summary>
        /// <param name="trade"></param>
        /// <returns></returns>
        [HttpPut("update/trade")]
        public ActionResult<Trade> UpdateTrade([FromBody] Trade trade) //to-do add existing tradeid validation
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = _tradeService.SubmitTradeDetails(trade, DBContants.action_update);
            return Ok(response);
        }

        /// <summary>
        /// Fetches all the trades 
        /// </summary>
        /// <returns></returns>
        [HttpGet("trades")]
        public ActionResult<IEnumerable<Trade>> GetTrades() => Ok(_tradeService.ListTrades());

        /// <summary>
        /// Fetches the positions of all assets in all accounts
        /// </summary>
        /// <returns></returns>
        [HttpGet("positions")]
        public ActionResult<IDictionary<string, Position>> GetPositions() => Ok(_tradeService.GetAllPositions());

        /// <summary>
        /// Fetches the position of an asset
        /// </summary>
        /// <param name="Asset"></param>
        /// <param name="Account"></param>
        /// <returns></returns>
        [HttpGet("positions/{Account}/{Asset}")]
        public ActionResult<Position> GetPosition(string Account, string Asset)
        {
            var pos = _tradeService.GetPosition(Account, Asset);
            if (pos == null) return NotFound();
            return Ok(pos);
        }
    }
}
