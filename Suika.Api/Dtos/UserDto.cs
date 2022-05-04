namespace Suika.Api.Dtos;

public record UserDto
{
    public string Username { get; init; } = default!;
    public string Email { get; init; } = default!;
}