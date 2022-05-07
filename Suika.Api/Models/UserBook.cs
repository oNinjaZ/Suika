namespace Suika.Api.Models;

public class UserBook
{
    public int UserBooksId { get; set; }
    public string Status { get; set; } = default!;
    public int UserId { get; set; }
    public int BookId { get; set; }
}