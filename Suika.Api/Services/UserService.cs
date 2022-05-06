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

    public async Task<bool> DeleteAsync(string username)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var result = await connection.ExecuteAsync(
            @"DELETE From Users WHERE Username = @Username",
            new { Username = username });
        return result > 0;

    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryAsync<User>(@"SELECT * FROM Users");
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QuerySingleOrDefaultAsync<User>(
            @"SELECT * FROM Users
            WHERE Username = @Username",
            new { Username = username });
    }

    public async Task<IEnumerable<User>> SearchByUsernameAsync(string searchTerm)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryAsync<User>(
            @"SELECT * FROM Users
            WHERE Username LIKE '%' || @SearchTerm || '%'",
            new { SearchTerm = searchTerm }
        );
    }

    public async Task<bool> UpdateAsync(User user)
    {

        using var connection = await _connectionFactory.CreateConnectionAsync();
        var result = await connection.ExecuteAsync(
            @"UPDATE Users
            SET Email = @Email
            WHERE Username = @Username",
            user);
        return result > 0;
    }
}