namespace Suika.Api.Models;

public class User
{
    public int UserId { get; set; }
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public DateTime RegistrationDate { get; } = DateTime.UtcNow;
}