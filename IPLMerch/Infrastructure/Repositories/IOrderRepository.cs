using IPLMerch.Application.Models;

namespace IPLMerch.Infrastructure.Repositories;

public interface IOrderRepository
{
    public Task<IQueryable<Order>> GetOrdersByUserIdAsync(Guid userId);
}