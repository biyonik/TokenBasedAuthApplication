using TokenBasedAuthApplication.Core.DTOs;
using TokenBasedAuthApplication.SharedLibrary;
using TokenBasedAuthApplication.SharedLibrary.DTOs;

namespace TokenBasedAuthApplication.Core.Services.Authentication;

public interface IAuthenticationService
{
    Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto);
    Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken);
    Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken);
    Task<Response<ClientTokenDto>> CreateTokenForClientAsync(ClientTokenDto clientTokenDto);
}