﻿using TokenBasedAuthApplication.Core.Configuration;
using TokenBasedAuthApplication.Core.DTOs;
using TokenBasedAuthApplication.Core.Entities;

namespace TokenBasedAuthApplication.Core.Services.Authentication;

public interface ITokenService
{
    TokenDto CreateToken(AppUser user);
    ClientTokenDto CreateTokenForClient(Client client);
}