using LSE.TradeHub.Core.Models;

namespace LSE.TradeHub.Core.Interfaces;

public interface ITradeRecordService : IServiceBase<TradeRecord, int> {
    KeyValuePair<string, decimal>? GetMeanValueBySymbol(string symbol);
}
