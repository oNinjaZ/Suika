using Dapper;
using Suika.Api.Data;
using Suika.Api.Models;

namespace Suika.Api.Services;

public class UserService : IUserService
{
    private readonly IDbConnectionFactory _connectionFactory;

    public UserService(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> Create(User user)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var result = await connection.ExecuteAsync(
            @"INSERT INTO Users(Username, Email, RegistrationDate)
            VALUES(@Username, @Registration, @RegistrationDate)", user);
        return result > 0;

    }

    public Task<bool> Delete(string username)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<User>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetByUsermame(string username)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Update(User user)
    {
        throw new NotImplementedException();
    }
}