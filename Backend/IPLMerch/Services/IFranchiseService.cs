using IPLMerch.Application.Models;

namespace IPLMerch.Services;

public interface IFranchiseService
{
    Task<IEnumerable<Franchise>> GetAllFranchisesAsync();
}