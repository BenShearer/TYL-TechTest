using System.ComponentModel.DataAnnotations;

namespace LSE.TradeHub.Core.Models;

public abstract class EntityBase<TId> {
    [Key] public virtual TId Id { get; set; }
}
