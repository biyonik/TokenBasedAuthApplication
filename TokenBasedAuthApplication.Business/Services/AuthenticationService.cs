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

    public AuthenticationService(IOptions<List<Client>> optionsClient, UserManager<AppUser> userManager,
        ITokenService tokenService,
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
            await (await _userRefreshTokenRepository.GetAsync(x => x.UserId == user.Id, default))
                .SingleOrDefaultAsync();
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
        var isExistRefreshToken =
            await (await _userRefreshTokenRepository.GetAsync(x => x.Code == refreshToken, default))
                .SingleOrDefaultAsync();
        if (isExistRefreshToken == null)
            return await Task.Run(() => Response<TokenDto>.Fail("Refresh token not found!", 404, true));

        var user = await _userManager.FindByIdAsync(isExistRefreshToken.UserId.ToString());
        if (user == null) return await Task.Run(() => Response<TokenDto>.Fail("User Id not found!", 404, true));

        TokenDto token = _tokenService.CreateToken(user);
        isExistRefreshToken.Code = token.RefreshToken;
        isExistRefreshToken.Expiration = token.RefreshTokenExpiration;
        await _userRefreshTokenRepository.UpdateAsync(isExistRefreshToken, default);
        var result = await _unitOfWork.CommitAsync();
        return result
            ? Response<TokenDto>.Success(token, 200)
            : Response<TokenDto>.Fail("Token recreation failed!", 400, true);
    }

    public async Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken)
    {
        var isExistRefreshToken =
            await (await _userRefreshTokenRepository.GetAsync(x => x.Code == refreshToken, default))
                .SingleOrDefaultAsync();
        if (isExistRefreshToken == null)
            return await Task.Run(() => Response<NoDataDto>.Fail("Refresh token not found!", 404, true));

        await _userRefreshTokenRepository.DeleteAsync(isExistRefreshToken, default);
        var result = await _unitOfWork.CommitAsync();
        return result
            ? Response<NoDataDto>.Success(200)
            : Response<NoDataDto>.Fail("Token remove failed!", 400, true);
    }

    public Task<Response<ClientTokenDto>> CreateTokenForClientAsync(LoginForClientDto loginForClientDto)
    {
        if (loginForClientDto == null) throw new ArgumentNullException(nameof(loginForClientDto));

        var client = _clients.SingleOrDefault(x =>
            x.Id == loginForClientDto.ClientId && x.Secret == loginForClientDto.ClientSecret);
        if (client == null) return Task.FromResult(Response<ClientTokenDto>.Fail("ClientId or ClientSecret not found!", 404, true));

        var token = _tokenService.CreateTokenForClient(client);
        return Task.FromResult(Response<ClientTokenDto>.Success(token, 200));
    }
}