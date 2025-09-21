using IPLMerch.Application.Models;
using IPLMerch.Enums;
using IPLMerch.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IPLMerch.Infrastructure.Repositories;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(IPLShopDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm, Guid? franchiseId, ProductType? productType)
    {
        var query = _dbSet.Include(p => p.Franchise).AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            query = query.Where(p => 
                p.Name.ToLower().Contains(searchTerm) || 
                p.Description.ToLower().Contains(searchTerm) ||
                p.Franchise.Name.ToLower().Contains(searchTerm));
        }

        if (franchiseId.HasValue)
        {
            query = query.Where(p => p.FranchiseId == franchiseId.Value);
        }

        if (productType.HasValue)
        {
            query = query.Where(p => p.Type == productType.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<Product?> GetProductWithFranchiseAsync(Guid id)
    {
        return await _dbSet
            .Include(p => p.Franchise)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public override async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _dbSet.Include(p => p.Franchise).ToListAsync();
    }
}