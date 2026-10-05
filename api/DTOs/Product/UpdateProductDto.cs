using System.ComponentModel.DataAnnotations;

namespace ErpApi.DTOs.Product
{
    public class UpdateProductDto
    {
        [Required(ErrorMessage = "Ürün adı boş bırakılamaz.")]
        [StringLength(100, ErrorMessage = "Ürün adı en fazla 100 karakter olabilir.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "SKU (Stok kodu) zorunludur.")]
        public string Sku { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "Satış fiyatı 0'dan büyük olmalıdır.")]
        public decimal SalePrice { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Alış fiyatı 0'dan küçük olamaz.")]
        public decimal PurchasePrice { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Minimum stok seviyesi 0'dan küçük olamaz.")]
        public int MinStockLevel { get; set; }

        public string Unit { get; set; } = "adet";

        public bool IsActive { get; set; }

        public int CategoryId { get; set; }
    }
}