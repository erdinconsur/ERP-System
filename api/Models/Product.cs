using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpApi.Models;

[Table("products")]
public class Product
{
    [Column("id")]
    public int Id { get; set; }

    [Column("sku")]
    public string? Sku { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("category_id")]
    public int? CategoryId { get; set; }

    [Column("unit")]
    public string? Unit { get; set; }

    [Column("sale_price")]
    public decimal SalePrice { get; set; }

    [Column("purchase_price")]
    public decimal? PurchasePrice { get; set; }

    [Column("min_stock_level")]
    public decimal? MinStockLevel { get; set; }

    [Column("is_active")]
    public bool? IsActive { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties (İlişkiler)
    public Category? Category { get; set; }
    public ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();
    public ICollection<SalesOrderItem> SalesOrderItems { get; set; } = new List<SalesOrderItem>();
    public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
}