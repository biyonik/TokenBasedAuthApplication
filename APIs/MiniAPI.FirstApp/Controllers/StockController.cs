using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MiniAPI.FirstApp.Controllers;

[ApiController()]
[Route("api/v1/[controller]")]
[Produces("application/json")]
[Authorize(Roles = "Admin")]
public class StockController : ControllerBase
{
    [HttpGet]
    public Task<IActionResult> GetStocks()
    {
        var username = HttpContext?.User.Identity?.Name;
        var userId =
            HttpContext?.User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
        var data = new Tuple<string?, string?>(username, userId?.Value);
        return Task.FromResult<IActionResult>(Ok(data));
    }
}