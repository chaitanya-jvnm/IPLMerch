using IPLMerch.Application.Models;
using IPLMerch.Infrastructure.Repositories;

namespace IPLMerch.Services.CoreServices;

public class FranchiseService : IFranchiseService
{
    private readonly IGenericRepository<Franchise> _franchiseRepository;

    public FranchiseService(IGenericRepository<Franchise> franchiseRepository)
    {
        _franchiseRepository = franchiseRepository;
    }

    public async Task<IEnumerable<Franchise>> GetAllFranchisesAsync()
    {
        var franchises = await _franchiseRepository.GetAllAsync();
        return franchises;
    }
}