using Bogus;
using LSE.TradeHub.Core.Models;

namespace LSE.TradeHub.Utilities;

public class TradeDataGenerator : ITradeDataGenerator {
    public TradeRecord[] GenerateRecords(Stock[] stockList) {
        var records = new List<TradeRecord>();
        var faker = new Faker<TradeRecord>()
            .RuleFor(s => s.UnitPrice, f => f.Finance.Amount())
            .RuleFor(s => s.Quantity, f => f.Random.Decimal(1, 10000))
            .RuleFor(s => s.Timestamp, f => f.Date.RecentOffset(100))
            .RuleFor(s => s.TraderReference, f => f.Name.LastName());

        foreach (var stock in stockList) {
            var fakeData = faker.Generate(100).Select(r => {
                r.StockId = stock.Id;
                return r;
            }).ToList();

            records.AddRange(fakeData);
        }

        return records.ToArray();
    }
}
