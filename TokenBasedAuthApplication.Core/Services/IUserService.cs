using TokenBasedAuthApplication.Core.DTOs;
using TokenBasedAuthApplication.SharedLibrary;

namespace TokenBasedAuthApplication.Core.Services;

public interface IUserService
{
    Task<Response<AppUserDto>> CreateUserAsync(CreateUserDto createUserDto);
    Task<Response<AppUserDto>> GetUserByUserNameAsync(string userName);
    Task<Response<AppUserDto>> GetUserByEmailAsync(string email);
}