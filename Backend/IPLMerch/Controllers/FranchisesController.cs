using IPLMerch.Services;
using Microsoft.AspNetCore.Mvc;

namespace IPLMerch.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FranchisesController : ControllerBase
{
    private readonly IFranchiseService _franchiseService;

    public FranchisesController(IFranchiseService franchiseService)
    {
        _franchiseService = franchiseService;
    }

    [HttpGet]
    public async Task<IActionResult> GetFranchises()
    {
        var franchises = await _franchiseService.GetAllFranchisesAsync();
        if(franchises.Any()) return Ok(franchises);
        return NoContent();
    }
}