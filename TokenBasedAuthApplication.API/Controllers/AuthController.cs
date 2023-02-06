﻿using Microsoft.AspNetCore.Mvc;
using TokenBasedAuthApplication.Core.DTOs;

namespace TokenBasedAuthApplication.API.Controllers;

public class AuthController: BaseApiController
{
    private readonly IAuthenticationService _authenticationService;

    public AuthController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("create-token")]
    public async Task<IActionResult> CreateToken(LoginDto loginDto)
    {
        var response = await _authenticationService.CreateTokenAsync(loginDto);
        return await HandleResponse(response);
    }

    [HttpPost("create-token-for-client")]
    public async Task<IActionResult> CreateTokenForClient(LoginForClientDto loginForClientDto)
    {
        var response = await _authenticationService.CreateTokenForClientAsync(loginForClientDto);
        return await HandleResponse(response);
    }
}