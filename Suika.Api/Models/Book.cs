namespace Suika.Api.Models;

public class Book
{
    public int BookId { get; set; }
    public string Title { get; set; } = default!;
    public string Type { get; set; } = default!;
    public int PageCount { get; set; }
    public DateTime ReleaseDate { get; set; }
}