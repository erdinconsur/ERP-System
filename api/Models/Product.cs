using System;
using System.Collections.Generic;

namespace ErpApi.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Sku { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int CategoryId { get; set; }

    public string Unit { get; set; } = null!;

    public decimal SalePrice { get; set; }

    public decimal PurchasePrice { get; set; }

    public decimal MinStockLevel { get; set; }

    public bool? IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    // Navigation property - EF Core doğrulamasına takılmaması için nullable (?) yapıldı
    public virtual Category? Category { get; set; }

    public virtual ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();

    public virtual ICollection<SalesOrderItem> SalesOrderItems { get; set; } = new List<SalesOrderItem>();

    public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
}