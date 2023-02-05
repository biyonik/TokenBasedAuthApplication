using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TokenBasedAuthApplication.Business.Services;

public static class SignService
{
    public static SecurityKey GetSymmetricSecurityKey(string securityKey)
    {
        var byteCode = Encoding.UTF8.GetBytes(securityKey);
        return new SymmetricSecurityKey(byteCode);
    }
}