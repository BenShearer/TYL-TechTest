using LSE.TradeHub.Core.Models;

namespace LSE.TradeHub.Utilities;

public interface IStockLoader {
    Stock[] GetStockList();
}
