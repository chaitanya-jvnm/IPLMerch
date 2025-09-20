using System.Net;
using IPLMerch.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace IPLMerch.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController
{
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> SearchProducts([FromQuery] string? name, [FromQuery] int? type, [FromQuery] string? franchise)
    {
        return new StatusCodeResult(StatusCodes.Status200OK);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        return new StatusCodeResult(StatusCodes.Status200OK);
    }

    [HttpPut]
    public async Task<IActionResult> AddProduct(Product product)
    {
        return new StatusCodeResult(StatusCodes.Status200OK);
    }
}