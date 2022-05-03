namespace Suika.Api.Models;

public class User
{
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public DateTime RegistrationDate { get; set; }
}