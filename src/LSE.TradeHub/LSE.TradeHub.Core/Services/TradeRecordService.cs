using LSE.TradeHub.Core.Interfaces;
using LSE.TradeHub.Core.Models;

namespace LSE.TradeHub.Core.Services;

public class TradeRecordService : ServiceBase<TradeRecord, int>, ITradeRecordService {
    public TradeRecordService(TradeDataContext dataContext) : base(dataContext) { }

    public KeyValuePair<string, decimal>? GetMeanValueBySymbol(string symbol) {
        if (!DataSet.Any(x => x.StockId == symbol)) {
            return null;
        }

        var meanPrice = DataSet.Where(x => x.StockId == symbol).Average(x => x.UnitPrice);

        return new KeyValuePair<string, decimal>(symbol, meanPrice);
    }

}
