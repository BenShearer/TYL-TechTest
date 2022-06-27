using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LSE.TradeHub.Core.Models;

[Index(nameof(StockId))]
public class TradeRecord : EntityBase<int> {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public override int Id { get; set; }

    [Required] [ForeignKey(nameof(Stock))] public string StockId { get; set; }

    [Required] public decimal Quantity { get; set; }

    [Required] public decimal UnitPrice { get; set; }

    [Required] public DateTimeOffset Timestamp { get; set; }

    [Required] public string TraderReference { get; set; }

    public virtual Stock Stock { get; set; }
}
