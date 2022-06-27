using LSE.TradeHub.Core.Models;
using LSE.TradeHub.Core.Services;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace LSE.TradeHub.Core.Tests {
    public class TradeRecordServiceTests {
        private readonly TradeRecordService tradeRecordService;

        private const string TEST_SYMBOL_1 = "TEST_1";
        private const string TEST_SYMBOL_2 = "TEST_2";

        private TradeRecord[] TestRecords => new[] {
            new TradeRecord { StockId = TEST_SYMBOL_1, UnitPrice = 1, TraderReference = "TEST_REF", Quantity = 1, Timestamp = DateTime.UtcNow },
            new TradeRecord { StockId = TEST_SYMBOL_1, UnitPrice = 2, TraderReference = "TEST_REF", Quantity = 1, Timestamp = DateTime.UtcNow },
            new TradeRecord { StockId = TEST_SYMBOL_1, UnitPrice = 3, TraderReference = "TEST_REF", Quantity = 1, Timestamp = DateTime.UtcNow },
            new TradeRecord { StockId = TEST_SYMBOL_2, UnitPrice = 5, TraderReference = "TEST_REF", Quantity = 1, Timestamp = DateTime.UtcNow },
            new TradeRecord { StockId = TEST_SYMBOL_2, UnitPrice = 10, TraderReference = "TEST_REF", Quantity = 1, Timestamp = DateTime.UtcNow },
            new TradeRecord { StockId = TEST_SYMBOL_2, UnitPrice = 15, TraderReference = "TEST_REF", Quantity = 1, Timestamp = DateTime.UtcNow },
        };

        public TradeRecordServiceTests() {
            var options = new DbContextOptionsBuilder<TradeDataContext>()
                .UseInMemoryDatabase(databaseName: "tradedata")
                .Options;
            var dataContext = new TradeDataContext(options);
            dataContext.TradeRecords.AddRange(TestRecords);
            dataContext.SaveChanges();

            tradeRecordService = new TradeRecordService(dataContext);
        }

        [Fact]
        public void GetAllMeanStockValues_Returns_Correct_Values() {
            var result = tradeRecordService.GetAllStockMeanValues();

            result[TEST_SYMBOL_1].ShouldBe(2);
            result[TEST_SYMBOL_2].ShouldBe(10);
        }

        [Theory]
        [InlineData(TEST_SYMBOL_1, 2)]
        [InlineData(TEST_SYMBOL_2, 10)]
        public void GetMeanStockValuesBySymbol_Returns_Correct_Value(string symbol, decimal expectedValue) {
            var result = tradeRecordService.GetMeanValueBySymbol(symbol);

            result.Value.Value.ShouldBe(expectedValue);
        }

        [Fact]
        public void GetMeanStockValueBySymbol_With_Nonexistent_Symbol_Returns_Null() {
            var result = tradeRecordService.GetMeanValueBySymbol("I DON'T EXIST");

            result.ShouldBe(null);
        }
    }
}
