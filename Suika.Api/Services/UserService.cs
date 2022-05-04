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

    public async Task<bool> CreateAsync(User user)
    {
        var existingUser = await GetByUsernameAsync(user.Username);
        if (existingUser is not null) return false;

        using var connection = await _connectionFactory.CreateConnectionAsync();
        var result = await connection.ExecuteAsync(
            @"INSERT INTO Users(Username, Email, RegistrationDate)
            VALUES (@Username, @Email, @RegistrationDate)",
            user);
        return result > 0;

    }

    public Task<bool> DeleteAsync(string username)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<User>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryFirstOrDefaultAsync<User>(@"
            SELECT * FROM Users
            WHERE Username = @Username",
            new {Username = username});
    }

    public Task<bool> UpdateAsync(User user)
    {
        throw new NotImplementedException();
    }
}