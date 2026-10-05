using System;
using System.Collections.Generic;

namespace ErpApi.Models;

public partial class SalesOrder
{
    public int Id { get; set; }

    public string OrderNo { get; set; } = null!;

    public int CustomerId { get; set; }

    public string Status { get; set; } = null!;

    public decimal TotalAmount { get; set; }

    public int CreatedBy { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<SalesOrderItem> SalesOrderItems { get; set; } = new List<SalesOrderItem>();
}
