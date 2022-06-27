using LSE.TradeHub.Core.Models;

namespace LSE.TradeHub.Utilities;

public interface ITradeDataGenerator {
    TradeRecord[] GenerateRecords(Stock[] stockList);
}
