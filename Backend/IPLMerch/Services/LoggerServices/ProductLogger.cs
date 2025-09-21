using System.Diagnostics;
using IPLMerch.Application.DTOs;
using IPLMerch.Services.CoreServices;

namespace IPLMerch.Services.LoggerServices;

public class ProductLogger : IProductService
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductLogger> _logger;

    public ProductLogger(IProductService productService, ILogger<ProductLogger> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    public async Task<ProductDto?> GetProductByIdAsync(Guid id)
    {
        _logger.LogInformation("Starting GetProductByIdAsync for ID: {ProductId}", id);
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        try
        {
            var result = await _productService.GetProductByIdAsync(id);
            _logger.LogInformation("GetProductByIdAsync completed in {ElapsedMs}ms. Found: {Found}",
                stopwatch.ElapsedMilliseconds, result != null);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetProductByIdAsync failed for ID: {ProductId}", id);
            throw;
        }
        finally
        {
            stopwatch.Stop();
        }
    }

    public async Task<IEnumerable<ProductDto>> SearchProductsAsync(SearchProductsDto searchDto)
    {
        _logger.LogInformation("Starting SearchProductsAsync");
        var stopwatch = Stopwatch.StartNew();

        try
        {
            var result = await _productService.GetAllProductsAsync();
            var count = result?.Count() ?? 0;
            _logger.LogInformation("GetAllProductsAsync completed in {ElapsedMs}ms. Count: {Count}",
                stopwatch.ElapsedMilliseconds, count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SearchProductsAsync failed");
            throw;
        }
        finally
        {
            stopwatch.Stop();
        }
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        _logger.LogInformation("Starting GetAllProductsAsync");
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        try
        {
            var result = await _productService.GetAllProductsAsync();
            var allProductsAsync = result.ToList();
            var count = allProductsAsync?.Count ?? 0;
            _logger.LogInformation("GetAllProductsAsync completed in {ElapsedMs}ms. Count: {Count}",
                stopwatch.ElapsedMilliseconds, count);
            return allProductsAsync;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetAllProductsAsync failed");
            throw;
        }
        finally
        {
            stopwatch.Stop();
        }
    }
}