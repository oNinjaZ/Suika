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
                UserId INTEGER PRIMARY KEY,
                Username TEXT NOT NULL,
                Email TEXT,
                RegistrationDate TEXT NOT NULL);

            CREATE TABLE IF NOT EXISTS Books(
                BookId INTEGER PRIMARY KEY,
                Title TEXT NOT NULL,
                Type TEXT NOT NULL,
                PageCount INTEGER);");

                // CREATE TABLE IF NOT EXISTS UserBooks(
                // UserBooksId INTEGER NOT NULL PRIMARY KEY,
                // Status INTEGER NOT NULL,
                // UserId INTEGER NOT NULL,
                // FOREIGN KEY (UserId) REFERENCES Users(UserId));
    }
}