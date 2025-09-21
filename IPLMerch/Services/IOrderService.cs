using IPLMerch.Application.DTOs;

namespace IPLMerch.Services;

public interface IOrderService
{
    Task<OrderDto> CreateOrderAsync(Guid userId, IList<CartItemDto> cartItems, string address);
    Task<IEnumerable<OrderDto>> GetUserOrdersAsync(Guid userId);
    
}