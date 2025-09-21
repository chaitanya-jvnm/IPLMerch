using IPLMerch.Application.DTOs;

namespace IPLMerch.Services;

public interface IOrderService
{
    Task<OrderDto> CreateOrderAsync(Guid userId, CreateOrderDto address);
    Task<IEnumerable<OrderDto>> GetUserOrdersAsync(Guid userId);
    
}