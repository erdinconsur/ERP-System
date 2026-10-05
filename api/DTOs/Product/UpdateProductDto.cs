using System.ComponentModel.DataAnnotations;

namespace ErpApi.DTOs.Product;

public class UpdateProductDto
{
    [Required(ErrorMessage = "Ürün adı zorunludur.")]
    [StringLength(200, ErrorMessage = "Ürün adı en fazla 200 karakter olabilir.")]
    public string Name { get; set; } = string.Empty;

    public string? Sku { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Satış fiyatı 0'dan büyük olmalıdır.")]
    public decimal SalePrice { get; set; }

    public decimal? PurchasePrice { get; set; }
    public decimal? MinStockLevel { get; set; }
    public string? Unit { get; set; }
    public bool? IsActive { get; set; }
    public int? CategoryId { get; set; }
}