using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Trade_Position.Models;
using Trade_Position.Services;

namespace Trade_Position.Controllers
{
    [ApiController]
    [Route("api")]
    public class TradeController : ControllerBase
    {
        private readonly TradeService _tradeService;

        public TradeController(TradeService svc) => _tradeService = svc;

        [HttpPost("add/trade")]
        public ActionResult<Trade> SubmitTrade([FromBody] Trade trade)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _tradeService.AddTrade(trade);
            return Ok();
        }

        [HttpGet("trades")]
        public ActionResult<IEnumerable<Trade>> GetTrades() => Ok(_tradeService.ListTrades());

        [HttpGet("positions")]
        public ActionResult<IDictionary<string, Position>> GetPositions() => Ok(_tradeService.GetAllPositions());

        [HttpGet("positions/{Asset}")]
        public ActionResult<Position> GetPosition(string Asset)
        {
            var pos = _tradeService.GetPosition(Asset);
            if (pos == null) return NotFound();
            return Ok(pos);
        }
    }
}
