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

    public Task<bool> Create(Book book)
    {
        throw new NotImplementedException();
    }
}