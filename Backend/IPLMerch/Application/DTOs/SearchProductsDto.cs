namespace IPLMerch.Application.DTOs;

public class SearchProductsDto
{
    public string SearchTerm { get; set; }
    public Guid? FranchiseId { get; set; }
    public string? ProductType { get; set; }
}