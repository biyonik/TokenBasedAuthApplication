using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using TokenBasedAuthApplication.Core.Configuration;
using TokenBasedAuthApplication.Core.DTOs;
using TokenBasedAuthApplication.Core.Entities;
using TokenBasedAuthApplication.Core.Services.Authentication;
using TokenBasedAuthApplication.SharedLibrary.Authentication;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace TokenBasedAuthApplication.Business.Services;

public class TokenService : ITokenService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly CustomTokenOptions _customTokenOptions;

    public TokenService(UserManager<AppUser> userManager, IOptions<CustomTokenOptions> options)
    {
        _userManager = userManager;
        _customTokenOptions = options.Value;
    }
    private string CreateRefreshToken()
    {
        var numberByteArray = new byte[32];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(numberByteArray);
        return Convert.ToBase64String(numberByteArray);
    }
    private IEnumerable<Claim> GetClaims(AppUser user, List<string> audiences)
    {
        var userClaimList = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.Name, user.UserName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        userClaimList.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
        return userClaimList;
    }
    private IEnumerable<Claim> GetClaimsForClient(Client client)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, client.Id),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        claims.AddRange(client.Audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
        return claims;
    }
    private JwtSecurityToken GetJwtSecurityToken(AppUser user, DateTime accessTokenExpiration)
    {
        SecurityKey securityKey = SignService.GetSymmetricSecurityKey(_customTokenOptions.SecurityKey);
        SigningCredentials signingCredentials =
            new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
        JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
            issuer: _customTokenOptions.Issuer,
            expires: accessTokenExpiration,
            notBefore: DateTime.Now,
            claims: GetClaims(user, _customTokenOptions.Audience),
            signingCredentials: signingCredentials
        );
        return jwtSecurityToken;
    }
    public TokenDto CreateToken(AppUser user)
    {
        DateTime accessTokenExpiration = DateTime.Now.AddMinutes(_customTokenOptions.AccessTokenExpiration);
        var jwtSecurityToken = GetJwtSecurityToken(user, accessTokenExpiration);

        JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        string token = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);
        TokenDto tokenDto = new TokenDto
        (
            AccessToken:token,
            RefreshToken:CreateRefreshToken(),
            AccessTokenExpiration:accessTokenExpiration,
            RefreshTokenExpiration:DateTime.Now.AddMinutes(_customTokenOptions.RefreshTokenExpiration)
        );
        return tokenDto;
    }
    private JwtSecurityToken GetJwtSecurityTokenForClient(Client client, DateTime accessTokenExpiration)
    {
        SecurityKey securityKey = SignService.GetSymmetricSecurityKey(_customTokenOptions.SecurityKey);
        SigningCredentials signingCredentials =
            new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
        JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
            issuer: _customTokenOptions.Issuer,
            expires: accessTokenExpiration,
            notBefore: DateTime.Now,
            claims: GetClaimsForClient(client),
            signingCredentials: signingCredentials
        );
        return jwtSecurityToken;
    }

    public ClientTokenDto CreateTokenForClient(Client client)
    {
        DateTime accessTokenExpiration = DateTime.Now.AddMinutes(_customTokenOptions.AccessTokenExpiration);
        var jwtSecurityToken = GetJwtSecurityTokenForClient(client, accessTokenExpiration);

        JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        string token = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);
        ClientTokenDto tokenDto = new ClientTokenDto
        (
            AccessToken:token,
            AccessTokenExpiration:accessTokenExpiration
        );
        return tokenDto;
    }
}