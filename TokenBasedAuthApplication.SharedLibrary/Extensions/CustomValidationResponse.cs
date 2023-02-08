using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using TokenBasedAuthApplication.SharedLibrary.DTOs;

namespace TokenBasedAuthApplication.SharedLibrary.Extensions;

public static class CustomValidationResponse
{
    public static void UseCustomValidationResponse(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>((ApiBehaviorOptions options) =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState.Values
                    .Where(x => x.Errors.Count > 0)
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage);
                ErrorDto errorDto = new(errors.ToList(), true);
                var response = Response<NoContentResult>.Fail(errorDto, 400);
                return new BadRequestObjectResult(response);
            };
        });
    }
}