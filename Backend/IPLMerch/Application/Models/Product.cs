using IPLMerch.Enums;

namespace IPLMerch.Application.Models;

public class Product : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public ProductType Type { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int StockQuantity { get; set; }
    public bool IsAvailable { get; set; }
    public Guid FranchiseId { get; set; }
    public string SKU { get; set; }
    public bool IsAutographed { get; set; }
        
    public Franchise Franchise { get; set; }
    public ICollection<CartItem> CartItems { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
}