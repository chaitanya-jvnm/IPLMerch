using IPLMerch.Application.Models;
using IPLMerch.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IPLMerch.Infrastructure.Repositories;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    public OrderRepository(IPLShopDbContext context) : base(context)
    {
    }

    public async Task<IQueryable<Order>> GetOrdersByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await Task.Run(() => _dbSet.Include(o => o.Items)
            .ThenInclude(p => p.Product)
            .ThenInclude(p => p.Franchise)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .AsQueryable(), cancellationToken);
    }
}