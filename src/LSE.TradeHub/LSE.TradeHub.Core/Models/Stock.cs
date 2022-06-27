using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace LSE.TradeHub.Core.Models {
    [Index(nameof(Id))]
    public class Stock : EntityBase<string> {
        [Required] public string Name { get; set; }

        public virtual ICollection<TradeRecord> TradeRecords { get; set; }
    }
}
