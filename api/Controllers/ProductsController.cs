using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ErpApi.Data;
using ErpApi.DTOs.Product;
using ErpApi.Models;

namespace ErpApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
    {
        var products = await _context.Products
            .Include(p => p.Category)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Sku = p.Sku,
                SalePrice = p.SalePrice,
                PurchasePrice = p.PurchasePrice,
                MinStockLevel = p.MinStockLevel,
                Unit = p.Unit,
                IsActive = p.IsActive,
                CategoryId = p.CategoryId,
                CategoryName = p.Category != null ? p.Category.Name : null,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            })
            .ToListAsync();

        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .Where(p => p.Id == id)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Sku = p.Sku,
                SalePrice = p.SalePrice,
                PurchasePrice = p.PurchasePrice,
                MinStockLevel = p.MinStockLevel,
                Unit = p.Unit,
                IsActive = p.IsActive,
                CategoryId = p.CategoryId,
                CategoryName = p.Category != null ? p.Category.Name : null,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            })
            .FirstOrDefaultAsync();

        if (product == null)
            return NotFound(new { message = $"{id} ID'li ürün bulunamadı." });

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto createDto)
    {
        var product = new Product
        {
            Name = createDto.Name,
            Sku = createDto.Sku,
            SalePrice = createDto.SalePrice,
            PurchasePrice = createDto.PurchasePrice,
            MinStockLevel = createDto.MinStockLevel,
            Unit = createDto.Unit,
            IsActive = createDto.IsActive, // Düzeltildi: '?? true' kaldırıldı
            CategoryId = createDto.CategoryId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var createdDto = await _context.Products
            .Include(p => p.Category)
            .Where(p => p.Id == product.Id)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Sku = p.Sku,
                SalePrice = p.SalePrice,
                PurchasePrice = p.PurchasePrice,
                MinStockLevel = p.MinStockLevel,
                Unit = p.Unit,
                IsActive = p.IsActive,
                CategoryId = p.CategoryId,
                CategoryName = p.Category != null ? p.Category.Name : null,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            })
            .FirstAsync();

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, createdDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, UpdateProductDto updateDto)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return NotFound(new { message = $"{id} ID'li ürün bulunamadı." });

        product.Name = updateDto.Name;
        product.Sku = updateDto.Sku;
        product.SalePrice = updateDto.SalePrice;
        product.PurchasePrice = updateDto.PurchasePrice;
        product.MinStockLevel = updateDto.MinStockLevel;
        product.Unit = updateDto.Unit;
        product.IsActive = updateDto.IsActive;
        product.CategoryId = updateDto.CategoryId;
        product.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return NotFound(new { message = $"{id} ID'li ürün bulunamadı." });

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}