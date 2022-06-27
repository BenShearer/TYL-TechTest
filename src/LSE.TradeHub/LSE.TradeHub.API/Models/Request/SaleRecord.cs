using System.ComponentModel.DataAnnotations;

namespace LSE.TradeHub.API.Models.Request {
    public class SaleRecord {
        [Required] public string StockSymbol { get; set; }

        [Required] public decimal Quantity { get; set; }

        [Required] public decimal TotalSaleValue { get; set; }

        [Required] public string TraderReference { get; set; }
    }
}
