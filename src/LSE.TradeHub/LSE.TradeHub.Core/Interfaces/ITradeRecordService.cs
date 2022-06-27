using LSE.TradeHub.Core.Models;

namespace LSE.TradeHub.Core.Interfaces;

public interface ITradeRecordService : IServiceBase<TradeRecord, int> {
    Dictionary<string, decimal> GetAllStockMeanValues();
    KeyValuePair<string, decimal>? GetMeanValueBySymbol(string symbol);
}
