using Dapper;
using Suika.Api.Data;
using Suika.Api.Models;

namespace Suika.Api.Services;

public class UserBookService : IUserBookService
{
    private readonly IDbConnectionFactory _connectionFactory;

    public UserBookService(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> CreateAsync(UserBook userBook)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var result = await connection.ExecuteAsync(
            @"INSERT INTO UserBooks(Status, UserId, BookId)
            VALUES(@Status, @UserId, @BookId)", userBook);
        return result > 0;
    }

    public async Task<IEnumerable<object>> GetAllAsync(string username)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryAsync<object>(
            @"SELECT
                Books.Title AS title,
                UserBooks.Status AS status
            FROM Users
                INNER JOIN UserBooks ON UserBooks.UserId = Users.UserId
                INNER JOIN Books ON Books.BookId = UserBooks.BookId
            WHERE Username = @Username", new { Username = username });
    }
}