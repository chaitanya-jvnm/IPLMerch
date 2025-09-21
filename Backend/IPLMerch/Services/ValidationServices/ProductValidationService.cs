using IPLMerch.Application.DTOs;
using IPLMerch.Application.Models;
using IPLMerch.Services.CoreServices;

namespace IPLMerch.Services.ValidationServices;

public class ProductValidationService : IProductService
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductValidationService> _logger;

    public ProductValidationService(IProductService productService, ILogger<ProductValidationService> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    public async Task<ProductDto?> GetProductByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            _logger.LogWarning("GetProductByIdAsync called with empty GUID");
            throw new ArgumentException("Product ID cannot be empty", nameof(id));
        }

        return await _productService.GetProductByIdAsync(id);
    }

    public async Task<IEnumerable<ProductDto>> SearchProductsAsync(SearchProductsDto searchDto)
    {
        return await _productService.SearchProductsAsync(searchDto);
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        return await _productService.GetAllProductsAsync();
    }
    
}