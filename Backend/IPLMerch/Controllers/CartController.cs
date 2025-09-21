using IPLMerch.Application.DTOs;
using IPLMerch.Services;
using Microsoft.AspNetCore.Mvc;

namespace IPLMerch.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        var cart = await _cartService.GetCartAsync(GetUserId());
        return Ok(cart);
    }

    [HttpPost("items")]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartDto dto)
    {
        try
        {
            var cart = await _cartService.AddToCartAsync(GetUserId(), dto);
            return Ok(cart);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("items/{productId}")]
    public async Task<IActionResult> UpdateCartItem(Guid productId, [FromBody] UpdateCartItemDto dto)
    {
        try
        {
            var cart = await _cartService.UpdateCartItemAsync(GetUserId(), productId, dto);
            return Ok(cart);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("items/{productId}")]
    public async Task<IActionResult> RemoveFromCart(Guid productId)
    {
        try
        {
            var cart = await _cartService.RemoveFromCartAsync(GetUserId(), productId);
            return Ok(cart);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete]
    public async Task<IActionResult> ClearCart()
    {
        await _cartService.ClearCartAsync(GetUserId());
        return NoContent();
    }
    
    private Guid GetUserId()
    {
        var userIdHeader = Request.Headers["X-User-Id"].ToString();
        if (Guid.TryParse(userIdHeader, out var userId))
        {
            return userId;
        }

        // Default user for demo
        return Guid.Parse("99999999-9999-9999-9999-999999999999");
    }
}