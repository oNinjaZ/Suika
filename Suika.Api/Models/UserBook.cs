namespace Suika.Api.Models;

public class UserBook
{
    public int UserBooksId { get; set; }
    public Status Status { get; set; }
    public int UserId { get; set; }
    public int BookId { get; set; }
}

public enum Status
{
    読みたい, いま読んでる, 読み終わった, 積読
}