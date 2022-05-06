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

    public Task<IEnumerable<UserBook>> GetAllAsync()
    {
        throw new NotImplementedException();
    }
}