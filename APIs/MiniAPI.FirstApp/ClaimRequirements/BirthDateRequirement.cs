using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace MiniAPI.FirstApp.ClaimRequirements;

public class BirthDateRequirement: IAuthorizationRequirement
{
    public BirthDateRequirement(int age)
    {
        Age = age;
    }

    public int Age { get; set; }
}

public class BirthDateRequirementHandler : AuthorizationHandler<BirthDateRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, BirthDateRequirement requirement)
    {
        Claim? birthDate = context.User.FindFirst("BirthDate");
        if (birthDate == null)
        {
            context.Fail();
            return;
        }
        
        var today = DateTime.Now;
        var age = today.Year - Convert.ToDateTime(birthDate.Value).Year;
        if (age >= requirement.Age)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }
}