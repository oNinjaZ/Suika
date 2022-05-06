using Suika.Api.Models;
using Dapper;
using Suika.Api.Data;

namespace Suika.Api.Services;

public class BookService : IBookService
{
    private readonly IDbConnectionFactory _connectionFactory;

    public BookService(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> CreateAsync(Book book)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var result = await connection.ExecuteAsync(
            @"INSERT INTO Books(Title, Type, PageCount, ReleaseDate)
            VALUES (@Title, @Type, @PageCount, @ReleaseDate)", book);
        return result > 0;
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryAsync<Book>(
            @"SELECT * FROM Books");
    }
}