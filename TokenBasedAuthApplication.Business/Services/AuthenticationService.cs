using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TokenBasedAuthApplication.Core.Configuration;
using TokenBasedAuthApplication.Core.DataAccess.Common;
using TokenBasedAuthApplication.Core.DTOs;
using TokenBasedAuthApplication.Core.Entities;
using TokenBasedAuthApplication.Core.Services.Authentication;
using TokenBasedAuthApplication.Core.UnitOfWork;
using TokenBasedAuthApplication.SharedLibrary;
using TokenBasedAuthApplication.SharedLibrary.DTOs;

namespace TokenBasedAuthApplication.Business.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly List<Client> _clients;
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<UserRefreshToken> _userRefreshTokenRepository;

    public AuthenticationService(IOptions<List<Client>> optionsClient, UserManager<AppUser> userManager, ITokenService tokenService,
        IUnitOfWork unitOfWork, IGenericRepository<UserRefreshToken> userRefreshTokenRepository)
    {
        _clients = optionsClient.Value;
        _userManager = userManager;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
        _userRefreshTokenRepository = userRefreshTokenRepository;
    }

    public async Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto)
    {
        if (loginDto == null) throw new ArgumentNullException(nameof(loginDto));

        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null) return Response<TokenDto>.Fail("Email or password is wrong!", 404, true);
        
        var checkPasswordResult = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!checkPasswordResult) return Response<TokenDto>.Fail("Email or password is wrong!", 404, true);

        var token = _tokenService.CreateToken(user);
        var userRefreshToken =
            await (await _userRefreshTokenRepository.GetAsync(x => x.UserId == user.Id, default)).SingleOrDefaultAsync();
        if (userRefreshToken == null)
        {
            await _userRefreshTokenRepository.AddAsync(new UserRefreshToken
            {
                UserId = user.Id,
                Code = token.RefreshToken,
                Expiration = token.RefreshTokenExpiration
            }, default);
        }
        else
        {
            userRefreshToken.Code = token.RefreshToken;
            userRefreshToken.Expiration = token.RefreshTokenExpiration;
            await _userRefreshTokenRepository.UpdateAsync(userRefreshToken, default);
        }

        var result = await _unitOfWork.CommitAsync();
        return result
            ? Response<TokenDto>.Success(token, 200)
            : Response<TokenDto>.Fail("Token creation failed!", 400, true);
    }

    public async Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Response<ClientTokenDto>> CreateTokenForClientAsync(ClientTokenDto clientTokenDto)
    {
        throw new NotImplementedException();
    }
}