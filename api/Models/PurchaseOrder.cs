using System;
using System.Collections.Generic;

namespace ErpApi.Models;

public partial class PurchaseOrder
{
    public int Id { get; set; }

    public string OrderNo { get; set; } = null!;

    public int SupplierId { get; set; }

    public string Status { get; set; } = null!;

    public decimal TotalAmount { get; set; }

    public int CreatedBy { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();

    public virtual Supplier Supplier { get; set; } = null!;
}
