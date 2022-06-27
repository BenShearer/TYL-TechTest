using LSE.TradeHub.Core;
using LSE.TradeHub.Core.Interfaces;
using LSE.TradeHub.Core.Models;

namespace LSE.TradeHub.Utilities {
    public interface IDataSeeder {
        Task SeedData();
    }

    public class DataSeeder : IDataSeeder {
        private readonly TradeDataContext context;
        private readonly IStockLoader stockLoader;
        private readonly IDataSeeder<Stock> stockService;
        private readonly IDataSeeder<TradeRecord> tradeRecordService;
        private readonly ITradeDataGenerator tradeDataGenerator;
        private readonly SeederOptions options;

        public DataSeeder(TradeDataContext context, IStockLoader stockLoader, IDataSeeder<Stock> stockService, IDataSeeder<TradeRecord> tradeRecordService, ITradeDataGenerator tradeDataGenerator, SeederOptions options) {
            this.context = context;
            this.stockLoader = stockLoader;
            this.stockService = stockService;
            this.tradeRecordService = tradeRecordService;
            this.tradeDataGenerator = tradeDataGenerator;
            this.options = options;
        }

        public async Task SeedData() {
            if (!options.SeedData) {
                return;
            }

            context.Database.EnsureCreated();

            var stocks = stockLoader.GetStockList();

            var tradeData = tradeDataGenerator.GenerateRecords(stocks);

            await stockService.CreateRange(stocks, options.ForceSeed);
            await tradeRecordService.CreateRange(tradeData, options.ForceSeed);
        }
    }
}
