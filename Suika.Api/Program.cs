using System.Net;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Suika.Api.Auth;
using Suika.Api.Data;
using Suika.Api.Extensions;
using Suika.Api.Models;
using Suika.Api.Services;

#region Services
var builder = WebApplication.CreateBuilder(args);

//builder.Configuration.AddJsonFile("appsettings.Local.json", true, true);

builder.Services.AddAuthentication(ApiKeySchemeConstants.SchemeName)
    .AddScheme<ApiKeyAuthSchemeOptions, ApiKeyAuthHandler>(ApiKeySchemeConstants.SchemeName, _ => { });
builder.Services.AddAuthorization();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IBookService, BookService>()
    .AddSingleton<IUserService, UserService>()
    .AddSingleton<IUserBookService, UserBookService>();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddSingleton<IDbConnectionFactory>(_ => new SqliteConnectionFactory(
    builder.Configuration.GetValue<string>("Database:ConnectionString")
));
builder.Services.AddSingleton<DatabaseInitializer>();
#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

#region User endpoints
app.MapGet("/users", async (IUserService userService, string? searchTerm) =>
{
    if (searchTerm is not null && !string.IsNullOrWhiteSpace(searchTerm))
    {
        var matchedUsers = await userService.SearchByUsernameAsync(searchTerm);
        var matchedUsersAsDtos = matchedUsers.Select(user => user.AsDto());
        return Results.Ok(matchedUsersAsDtos);
    }
    var users = await userService.GetAllAsync();
    var usersAsDtos = users.Select(user => user.AsDto());
    return Results.Ok(usersAsDtos);
});

app.MapGet("/users/{username}", async (string username, IUserService userService) =>
{
    var user = await userService.GetByUsernameAsync(username);
    return user is not null ? Results.Ok(user.AsDto()) : Results.NotFound();
});

app.MapPost("/users",
    //[Authorize(AuthenticationSchemes = ApiKeySchemeConstants.SchemeName)]
    async (User user, IUserService userService, IValidator<User> validator) =>
{
    var validationResult = await validator.ValidateAsync(user);
    if (!validationResult.IsValid)
    {
        return Results.BadRequest(validationResult.Errors);
    }
    var created = await userService.CreateAsync(user);
    if (!created)
    {
        return Results.BadRequest(new List<ValidationFailure>
        {
            new ("Username", "This username already exists")
        });
    }
    return Results.Created($"/users/{user.Username}", user.AsDto());
});

app.MapPut("/users/{username}", async (string username, User user, IUserService userService,
    IValidator<User> validator) =>
{
    user.Username = username;
    var validationResult = await validator.ValidateAsync(user);
    if (!validationResult.IsValid)
    {
        return Results.BadRequest(validationResult.Errors);
    }
    var updated = await userService.UpdateAsync(user);
    return (updated) ? Results.NoContent() : Results.NotFound();
});

app.MapDelete("/users/{username}", async (string username, IUserService userService) =>
{
    return await userService.DeleteAsync(username) ? Results.NoContent() : Results.NotFound();
});
#endregion

#region Book endpoints
app.MapPost("/books", async (Book book, IBookService bookService) =>
{
    //todo add validator

    var created = await bookService.CreateAsync(book);
    if (!created)
    {
        return Results.BadRequest(); // todo return validation error
    }

    return Results.Ok(); //todo change to Results.Created() after mapping Get(single book) endpoint
});

app.MapGet("/books", async (IBookService bookService) =>
{
    var books = await bookService.GetAllAsync();
    return Results.Ok(books);
});
#endregion

#region UserBooks endpoints 
app.MapPost("/{username}/books", async (string username, UserBook userBook, IUserService userService, IUserBookService userBookService) =>
{
    var existingUser = await userService.GetByUsernameAsync(username);
    if(existingUser is null) return Results.BadRequest("User not found");

    userBook.UserId = existingUser.UserId;
    var created = await userBookService.CreateAsync(userBook);
    if (!created) return Results.BadRequest();
    return Results.Ok();
});

app.MapGet("/{username}/books", async (string username, IUserBookService userBookService) =>
{
    var userBooks = await userBookService.GetAllAsync(username);
    return Results.Ok(userBooks);
});
#endregion

var databaseInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
await databaseInitializer.InitializeAsync();

app.Run();
