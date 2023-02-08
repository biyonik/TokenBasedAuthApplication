using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using TokenBasedAuthApplication.SharedLibrary.DTOs;
using TokenBasedAuthApplication.SharedLibrary.Exceptions;

namespace TokenBasedAuthApplication.SharedLibrary.Extensions;

public static class CustomExceptionHandler
{
    public static void UseCustomException(this WebApplication app)
    {
        app.UseExceptionHandler(configure =>
        {
            configure.Run(async context =>
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (errorFeature != null)
                {
                    var ex = errorFeature.Error;
                    ErrorDto errorDto = null;
                    if (ex is CustomException)
                    {
                        errorDto = new(ex.Message, true);
                    }
                    else
                    {
                        errorDto = new(ex.Message, false);
                    }

                    var response = Response<NoDataDto>.Fail(errorDto, 500);
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                }
            });
        });
    }
}