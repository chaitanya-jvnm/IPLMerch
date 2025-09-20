namespace IPLMerch.Application.DTOs;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string Type { get; set; }
    public string ImageUrl { get; set; }
    public int StockQuantity { get; set; }
    public bool IsAvailable { get; set; }
    public string SKU { get; set; }
    public bool IsAutographed { get; set; }
    public FranchiseDto Franchise { get; set; }
}