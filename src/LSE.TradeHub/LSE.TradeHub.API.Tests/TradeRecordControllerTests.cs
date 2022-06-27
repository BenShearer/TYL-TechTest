using AutoMapper;
using LSE.TradeHub.API.Configuration;
using LSE.TradeHub.API.Controllers;
using LSE.TradeHub.API.Models.Request;
using LSE.TradeHub.Core.Interfaces;
using LSE.TradeHub.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace LSE.TradeHub.API.Tests {
    public class TradeRecordControllerTests {
        private readonly Mock<ITradeRecordService> tradeRecordServiceMock;
        private readonly Mock<ISystemClock> clockMock;
        private readonly Mock<ILogger> loggerMock;
        private readonly IMapper mapper;

        public TradeRecordControllerTests() {
            tradeRecordServiceMock = new Mock<ITradeRecordService>();
            clockMock = new Mock<ISystemClock>();
            loggerMock = new Mock<ILogger>();

            mapper = new MapperConfiguration(c =>
                c.AddProfile<AutomapperProfile>()).CreateMapper();
        }

        [Fact]
        public async Task NullInput_Returns_BadRequest() {
            var controller = ConstructController();

            var result = await controller.AddTradeRecord(null);

            result.ShouldBeOfType(typeof(BadRequestObjectResult));
        }

        [Fact]
        public async Task AddTradeRecord_SetsTimestamp() {
            var testDate = new DateTime(1982, 11, 26, 11, 11, 11);

            clockMock.Setup(svc => svc.UtcNow).Returns(testDate.ToUniversalTime()).Verifiable();

            var controller = ConstructController();

            var model = new SaleRecord {
                Quantity = 100,
                StockSymbol = "TEST_SYMBOL",
                TotalSaleValue = 100,
                TraderReference = "TEST_TRADER"
            };

            await controller.AddTradeRecord(model);

            clockMock.VerifyAll();
            tradeRecordServiceMock.Verify(svc =>
                    svc.Create(
                        It.Is<TradeRecord>(x =>
                            x.Timestamp == testDate.ToUniversalTime()
                        )
                    )
                , Times.Once);
        }

        [Fact]
        public async Task AddTradeRecord_Sets_Unit_Price_Correctly() {
            var controller = ConstructController();

            var model = new SaleRecord {
                Quantity = 50,
                StockSymbol = "TEST_SYMBOL",
                TotalSaleValue = 100,
                TraderReference = "TEST_TRADER"
            };

            await controller.AddTradeRecord(model);

            tradeRecordServiceMock.Verify(svc =>
                    svc.Create(
                        It.Is<TradeRecord>(x => x.UnitPrice == 2)
                    )
                , Times.Once);
        }

        [Fact]
        public async Task AddTradeRecord_Returns_BadRequest_On_DBUpdateException() {
            tradeRecordServiceMock.Setup(svc => svc.Create(It.IsAny<TradeRecord>())).Throws<DbUpdateException>();

            var controller = ConstructController();

            var model = new SaleRecord {
                Quantity = 100,
                StockSymbol = "TEST_SYMBOL",
                TotalSaleValue = 100,
                TraderReference = "TEST_TRADER"
            };

            var result = await controller.AddTradeRecord(model);
            result.ShouldBeOfType(typeof(BadRequestResult));
        }

        private TradeRecordsController ConstructController() {
            return new TradeRecordsController(
                tradeRecordServiceMock.Object,
                clockMock.Object,
                mapper,
                loggerMock.Object);
        }
    }
}
