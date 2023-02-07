namespace TokenBasedAuthApplication.SharedLibrary.DTOs;

public sealed record ErrorDto
{
    public List<string> Errors { get; private set; }
    public bool IsShow { get; private set; }

    public ErrorDto()
    {
        Errors = new();
    }

    public ErrorDto(string error, bool isShow)
    {
        Errors ??= new List<string>();
        Errors.Add(error);
        IsShow = isShow;
    }

    public ErrorDto(List<string> errors, bool isShow)
    {
        Errors = errors;
        IsShow = isShow;
    }
}