using IPLMerch.Application.DTOs;
using IPLMerch.Services;
using IPLMerch.Services.CoreServices;
using Microsoft.AspNetCore.Mvc;

namespace IPLMerch.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _productService.GetAllProductsAsync();
        if(products.Any()) return Ok(products);
        return NoContent();
    }
    
    [HttpPost("search")]
    public async Task<IActionResult> SearchProducts([FromBody] SearchProductsDto searchDto)
    {
        var products = await _productService.SearchProductsAsync(searchDto);
        return Ok(products);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }
    
}