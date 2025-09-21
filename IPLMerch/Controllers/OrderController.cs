using IPLMerch.Application.DTOs;
using IPLMerch.Services;
using Microsoft.AspNetCore.Mvc;

namespace IPLMerch.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUserOrders()
    {
        var orders = await _orderService.GetUserOrdersAsync(GetUserId());
        return Ok(orders);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(List<CartItemDto> cartItems, string address)
    {
        var order = await _orderService.CreateOrderAsync(GetUserId(), cartItems, address);
        return Created(order.OrderNumber, order);
    }

    #region private
    
    private Guid GetUserId()
    {
        var userIdHeader = Request.Headers["X-User-Id"].ToString();
        return Guid.TryParse(userIdHeader, out var userId) ? userId : Guid.Parse("99999999-9999-9999-9999-999999999999");
    }

    #endregion
}