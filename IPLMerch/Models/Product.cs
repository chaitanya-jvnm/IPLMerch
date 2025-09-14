using IPLMerch.Enums;

namespace IPLMerch.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ProductType Type { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public string Description { get; set; }
    public int FranchiseId { get; set; }
    public Franchise Franchise { get; set; }
}