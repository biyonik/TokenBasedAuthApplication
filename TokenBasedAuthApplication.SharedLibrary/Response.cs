using System.Text.Json.Serialization;
using TokenBasedAuthApplication.SharedLibrary.DTOs;

namespace TokenBasedAuthApplication.SharedLibrary;

public record Response<T>
{
    public T? Data { get; init; }
    public int StatusCode { get; init; }
    public ErrorDto? Error { get; init; }
    
    [JsonIgnore]
    public bool IsSuccessful { get; private init; }
    
    public static Response<T> Success(T data, int statusCode)
    {
        return new Response<T>
        {
            Data = data,
            StatusCode = statusCode,
            IsSuccessful = true
        };
    }

    public static Response<T> Success(int statusCode)
    {
        return new Response<T>
        {
            Data = default,
            StatusCode = statusCode,
            IsSuccessful = true
        };
    }

    public static Response<T> Fail(ErrorDto errorDto, int statusCode)
    {
        return new Response<T>
        {
            StatusCode = statusCode,
            Error = errorDto,
            IsSuccessful = false
        };
    }

    public static Response<T> Fail(string error, int statusCode, bool isShow)
    {
        var errorDto = new ErrorDto(error, isShow);
        return new Response<T>
        {
            Error = errorDto,
            StatusCode = statusCode,
            IsSuccessful = false
        };
    }
}

