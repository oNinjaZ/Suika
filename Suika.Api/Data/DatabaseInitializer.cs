using Dapper;

namespace Suika.Api.Data;

public class DatabaseInitializer
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DatabaseInitializer(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task InitializeAsync()
    {
        var connection = await _connectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync(@"
            CREATE TABLE IF NOT EXISTS Users(
                Username TEXT NOT NULL PRIMARY KEY,
                Email TEXT NOT NULL,
                RegistrationDate TEXT NOT NULL);
            
            CREATE TABLE IF NOT EXISTS Books(
                Id TEXT NOT NULL PRIMARY KEY,
                Title TEXT NOT NULL,
                PageCount INTEGER,
                CompletionDate TEXT NOT NULL);

            CREATE TABLE UserBooks(
                Id TEXT PRIMARY KEY,
                Status INTEGER NOT NULL,
                CompletionDate TEXT,
                Username TEXT FOREIGN KEY REFERENCES Users(Username),
                BookId TEXT FOREIGN KEY REFERENCES Books(Id))
                ");

            
    }
}