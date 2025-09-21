using AutoMapper;
using IPLMerch.Application.DTOs;
using IPLMerch.Enums;
using IPLMerch.Infrastructure.Repositories;

namespace IPLMerch.Services.CoreServices;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }


    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<ProductDto?> GetProductByIdAsync(Guid id)
    {
        var product = await _productRepository.GetProductWithFranchiseAsync(id);
        return _mapper.Map<ProductDto>(product);
    }

    public async Task<IEnumerable<ProductDto>> SearchProductsAsync(SearchProductsDto searchDto)
    {
        ProductType? productType = null;
        if (!string.IsNullOrWhiteSpace(searchDto.ProductType))
        {
            Enum.TryParse<ProductType>(searchDto.ProductType, out var type);
            productType = type;
        }

        var products = await _productRepository.SearchProductsAsync(
            searchDto.SearchTerm,
            searchDto.FranchiseId,
            productType);

        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }
    
}