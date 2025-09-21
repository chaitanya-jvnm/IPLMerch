using IPLMerch.Application.Models;

namespace IPLMerch.Services.CoreServices;

public interface IFranchiseService
{
    Task<IEnumerable<Franchise>> GetAllFranchisesAsync();
}