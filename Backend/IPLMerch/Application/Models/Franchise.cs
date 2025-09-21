namespace IPLMerch.Application.Models;

public class Franchise : BaseEntity
{
    /// <summary>
    /// The Name of the Franchise
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// The Franchise Code
    /// </summary>
    public string Code { get; set; }
    
    /// <summary>
    /// The City the Franchise represents
    /// </summary>
    public string City { get; set; }
    
    /// <summary>
    /// The Products they offer
    /// </summary>
    public IEnumerable<Product> Products { get; set; }
}