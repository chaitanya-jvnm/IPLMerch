using IPLMerch.Application.Models;

namespace IPLMerch.Infrastructure.Repositories;

public interface IOrderRepository : IGenericRepository<Order>
{
    public Task<IQueryable<Order>> GetOrdersByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}