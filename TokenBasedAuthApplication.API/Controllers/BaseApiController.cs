using System.Net;
using Microsoft.AspNetCore.Mvc;
using TokenBasedAuthApplication.SharedLibrary;

namespace TokenBasedAuthApplication.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class BaseApiController: ControllerBase
{
    protected Task<IActionResult> HandleResponse<T>(Response<T> response)
    {
        return Task.FromResult<IActionResult>(new ObjectResult(response)
        {
            StatusCode = response.StatusCode
        });
    }
}