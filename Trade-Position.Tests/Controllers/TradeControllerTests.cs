using Microsoft.AspNetCore.Mvc;
using Moq;
using Trade_Position.Controllers;
using Trade_Position.Interfaces;
using Trade_Position.Models;

namespace Trade_Position.Trade_Position.Tests.Controllers
{
    public class TradeControllerTests
    {
        private readonly Mock<ITradeService> _tradeService = new();
        private readonly TradeController _tradeController;

        public TradeControllerTests() 
        {
            _tradeController = new TradeController(_tradeService.Object);
        }

        [Fact]
        public void TestAddTrad()
        {
            var trade = new Trade();
            var res = _tradeController.AddTrade(trade);
            ActionResult actionResult = Assert.IsType<ActionResult>(res);
        }
    }
}
