using System;

namespace ErpApi.DTOs.Product;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Sku { get; set; }
    public decimal SalePrice { get; set; }
    public decimal? PurchasePrice { get; set; }
    public decimal? MinStockLevel { get; set; }
    public string? Unit { get; set; }
    public bool? IsActive { get; set; }
    public int? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}