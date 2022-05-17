using System.Net;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Suika.Api.Auth;
using Suika.Api.Data;
using Suika.Api.Endpoints;
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

builder.Services.AddUserEndpoints();
builder.Services.AddBookEndpoints();
builder.Services.AddSingleton<IUserBookService, UserBookService>();

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

app.UseUserEndpoints();
app.UseBookEndpoints();

#region UserBooks endpoints 
app.MapPost("/{username}/books", async (string username, UserBook userBook, IUserService userService, IUserBookService userBookService) =>
{
    var existingUser = await userService.GetByUsernameAsync(username);
    if (existingUser is null) return Results.BadRequest("User not found");

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
