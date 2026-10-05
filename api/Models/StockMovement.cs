using System;
using System.Collections.Generic;

namespace ErpApi.Models;

public partial class StockMovement
{
    public long Id { get; set; }

    public int ProductId { get; set; }

    public decimal Quantity { get; set; }

    public string MovementType { get; set; } = null!;

    public string? ReferenceType { get; set; }

    public int? ReferenceId { get; set; }

    public string? Note { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
