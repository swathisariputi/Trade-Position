using Asp.Versioning;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using Trade_Position.Constants;
using Trade_Position.Models;
using Trade_Position.Interfaces;
using Trade_Position.Validators;

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
        private readonly ITradeService _tradeService;

        /// <summary>
        /// Constructor initiates TradeService
        /// </summary>
        /// <param name="svc"></param>
        public TradeController(ITradeService tradeService) => _tradeService = tradeService;

        /// <summary>
        /// Adds Trade using rest service
        /// </summary>
        /// <param name="trade"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPost("AddTrade"), MapToApiVersion(API_VERSION_01)]
        public ActionResult<Trade> AddTrade([FromBody] Trade trade)
        {
            if (!ModelState.IsValid) return StatusCode(StatusCodes.Status400BadRequest,ModelState.ToString());
            try
            {
                TradeValidator validator = new();
                ValidationResult validationResult = validator.Validate(trade);

                if (!validationResult.IsValid)
                {
                    var errorString = new StringBuilder();
                    foreach (var err in validationResult.Errors)
                    {
                        errorString.AppendLine(err.ErrorMessage);
                    }
                    return StatusCode(StatusCodes.Status400BadRequest, errorString);
                }
                var response = _tradeService.SubmitTradeDetails(trade, DBContants.action_add);
                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }

        /// <summary>
        /// updates Trade
        /// </summary>
        /// <param name="trade"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPut("UpdateTrade/{TradeId}"), MapToApiVersion(API_VERSION_01)]
        public ActionResult<Trade> UpdateTrade(int TradeId, [FromBody] Trade trade) //to-do add existing tradeid validation
        {
            if (!ModelState.IsValid) return StatusCode(StatusCodes.Status400BadRequest, ModelState.ToString());
            try
            {
                TradeValidator validator = new();
                ValidationResult validationResult = validator.Validate(trade);

                if (!validationResult.IsValid)
                {
                    var errorString = new StringBuilder();
                    foreach (var err in validationResult.Errors)
                    {
                        errorString.AppendLine(err.ErrorMessage);
                    }
                    return StatusCode(StatusCodes.Status400BadRequest, errorString);
                }
                if (trade.TradeId == 0)
                {
                    trade.TradeId = TradeId;
                }
                if(TradeId != trade.TradeId)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "TradeId should match with trade");
                }
                var response = _tradeService.SubmitTradeDetails(trade, DBContants.action_update);
                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }

        /// <summary>
        /// Fetches all the trades 
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(IEnumerable<Trade>), StatusCodes.Status200OK)]
        [HttpGet("trades"), MapToApiVersion(API_VERSION_01)]
        public ActionResult<IEnumerable<Trade>> GetTrades()
        {
            if (!ModelState.IsValid) return StatusCode(StatusCodes.Status400BadRequest, ModelState.ToString());
            try
            {
                var response = _tradeService.ListTrades();
                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }

        /// <summary>
        /// Fetches the positions of all assets in all accounts
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(IDictionary<string, Position>), StatusCodes.Status200OK)]
        [HttpGet("positions"), MapToApiVersion(API_VERSION_01)]
        public ActionResult<IDictionary<string, Position>> GetPositions()
        {
            if (!ModelState.IsValid) return StatusCode(StatusCodes.Status400BadRequest, ModelState.ToString());
            try
            {
                var response = _tradeService.GetAllPositions();
                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }

        /// <summary>
        /// Fetches the position of an asset
        /// </summary>
        /// <param name="Asset"></param>
        /// <param name="Account"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Exception), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(Position), StatusCodes.Status200OK)]
        [HttpGet("positions/{Account}/{Asset}"), MapToApiVersion(API_VERSION_01)]
        public ActionResult<Position> GetPosition(string Account, string Asset)
        {
            if (!ModelState.IsValid) return StatusCode(StatusCodes.Status400BadRequest, ModelState.ToString());
            try
            {
                var response = _tradeService.GetPosition(Account, Asset);
                if (response == null) return StatusCode(StatusCodes.Status404NotFound, "Postion not available for given account and asset");
                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        /// <summary>
        /// Fetches the trade of an asset
        /// </summary>
        /// <param name="Asset"></param>
        /// <param name="Account"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Exception), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(IEnumerable<Trade>), StatusCodes.Status200OK)]
        [HttpGet("trades/{Account}/{Asset}"), MapToApiVersion(API_VERSION_01)]
        public ActionResult<IEnumerable<Trade>> GetTrade(string Account, string Asset)
        {
            if (!ModelState.IsValid) return StatusCode(StatusCodes.Status400BadRequest, ModelState.ToString());
            try
            {
                var response = _tradeService.GetTrades(Account, Asset);
                if (response == null) return StatusCode(StatusCodes.Status404NotFound, "Trades not available for given account and asset");
                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}
