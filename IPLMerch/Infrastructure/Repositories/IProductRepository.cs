using IPLMerch.Application.Models;
using IPLMerch.Enums;

namespace IPLMerch.Infrastructure.Repositories;

public interface IProductRepository : IGenericRepository<Product>
{
    public Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm, Guid? franchiseId,
        ProductType? productType);

    public Task<Product?> GetProductWithFranchiseAsync(Guid id);
}