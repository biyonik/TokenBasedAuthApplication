using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TokenBasedAuthApplication.Core.DTOs;
using TokenBasedAuthApplication.SharedLibrary.Exceptions;

namespace TokenBasedAuthApplication.API.Controllers;

public class UsersController: BaseApiController
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("create-user")]
    public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
    {
        var response = await _userService.CreateUserAsync(createUserDto);
        return await HandleResponse(response);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUser()
    {
        return await HandleResponse(await _userService.GetUserByUserNameAsync(HttpContext.User.Identity.Name));
    }
}