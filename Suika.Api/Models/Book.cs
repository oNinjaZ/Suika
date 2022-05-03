namespace Suika.Api.Models;

public class Book
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public DateTime CompletionDate { get; set; }
    public int PageCount { get; set; }
}