using IPLMerch.Application.DTOs;

namespace IPLMerch.Services;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllProductsAsync();
    Task<ProductDto?> GetProductByIdAsync(Guid id);
    Task<IEnumerable<ProductDto>> SearchProductsAsync(SearchProductsDto searchDto);
}