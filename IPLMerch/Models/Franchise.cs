namespace IPLMerch.Models;

public class Franchise
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string LogoUrl { get; set; }
    public ICollection<Product> Products { get; set; }
}