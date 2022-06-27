using LSE.TradeHub.Core.Interfaces;
using LSE.TradeHub.Core.Models;

namespace LSE.TradeHub.Core.Services {
    public class StockService : ServiceBase<Stock, string>, IStockService {
        public StockService(TradeDataContext dataContext) : base(dataContext) { }
    }
}
