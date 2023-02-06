using Microsoft.AspNetCore.Identity;
using TokenBasedAuthApplication.Business.Mappings;
using TokenBasedAuthApplication.Core.DTOs;
using TokenBasedAuthApplication.Core.Entities;
using TokenBasedAuthApplication.Core.Services;
using TokenBasedAuthApplication.SharedLibrary;
using TokenBasedAuthApplication.SharedLibrary.DTOs;

namespace TokenBasedAuthApplication.Business.Services;

public class UserService: IUserService
{
    private readonly UserManager<AppUser> _userManager;

    public UserService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Response<AppUserDto>> CreateUserAsync(CreateUserDto createUserDto)
    {
        AppUser user = new AppUser
        {
            Email = createUserDto.Email,
            UserName = createUserDto.UserName
        };
        
        IdentityResult identityResult = await _userManager.CreateAsync(user, createUserDto.Password);
        if (!identityResult.Succeeded)
        {
            List<string> errors = identityResult.Errors.Select(x => x.Description).ToList();
            ErrorDto errorDto = new ErrorDto(errors, true);
            return Response<AppUserDto>.Fail(errorDto, 400);
        }

        AppUserDto appUserDto = ObjectMapper.Mapper.Map<AppUserDto>(user);
        return Response<AppUserDto>.Success(appUserDto, 200);
    }

    public async Task<Response<AppUserDto>> GetUserByUserNameAsync(string userName)
    {
        AppUser? user = await _userManager.FindByNameAsync(userName);
        if (user == null) return Response<AppUserDto>.Fail("User not found by username!", 400, true);
        
        AppUserDto appUserDto = ObjectMapper.Mapper.Map<AppUserDto>(user);
        return Response<AppUserDto>.Success(appUserDto, 200);
    } 

    public async Task<Response<AppUserDto>> GetUserByEmailAsync(string email)
    {
        AppUser? user = await _userManager.FindByEmailAsync(email);
        if (user == null) return Response<AppUserDto>.Fail("User not found by email!", 400, true);
        
        AppUserDto appUserDto = ObjectMapper.Mapper.Map<AppUserDto>(user);
        return Response<AppUserDto>.Success(appUserDto, 200);
    }
}