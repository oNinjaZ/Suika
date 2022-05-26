using FluentValidation;
using Suika.Api.Extensions;
using Suika.Api.Models;
using Suika.Api.Services;

namespace Suika.Api.Endpoints;

public static class UserEndpoints
{
    public static IServiceCollection AddUserEndpoints(this IServiceCollection services)
    {
        services.AddSingleton<IUserService, UserService>();
        return services;
    }

    public static IEndpointRouteBuilder UseUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/users", SearchUsersAsync)
            .WithName("SearchUsers");

        app.MapGet("/users/{username}", GetUserAsync)
            .WithName("GetUser");

        app.MapPost("/users", CreateUserAsync)
            .WithName("CreateUser");

        app.MapPut("/users/{username}", UpdateUserAsync)
            .WithName("UpdateUser");

        app.MapDelete("/users/{username}", DeleteUserAsync)
            .WithName("DeleteUser");

        return app;
    }

    internal static async Task<IResult> SearchUsersAsync(IUserService userService, string? searchTerm)
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
    }

    internal static async Task<IResult> GetUserAsync(string username, IUserService userService)
    {
        var user = await userService.GetByUsernameAsync(username);
        return user is not null ? Results.Ok(user.AsDto()) : Results.NotFound();
    }

    internal static async Task<IResult> CreateUserAsync(
        User user,
        IUserService userService,
        IValidator<User> validator)
    {
        var validationResult = await validator.ValidateAsync(user);
        if (!validationResult.IsValid)
        {
            return Results.BadRequest(validationResult.Errors.Select(e => new
            {
                propertyName = e.PropertyName,
                errorMessage = e.ErrorMessage
            }));
        }
        var created = await userService.CreateAsync(user);
        if (!created)
        {
            return Results.BadRequest(new
            {
                propertyName = "Username",
                errorMessage = "This username already exists."
            });
        }
        return Results.CreatedAtRoute("GetUser", new { username = user.Username }, user.AsDto());
    }

    internal static async Task<IResult> UpdateUserAsync(
        string username,
        User user,
        IUserService userService,
        IValidator<User> validator)
    {
        user.Username = username;
        var validationResult = await validator.ValidateAsync(user);
        if (!validationResult.IsValid)
        {
            return Results.BadRequest(validationResult.Errors);
        }
        var updated = await userService.UpdateAsync(user);
        return (updated) ? Results.NoContent() : Results.NotFound();
    }

    internal static async Task<IResult> DeleteUserAsync(string username, IUserService userService)
    {
        return await userService.DeleteAsync(username) ? Results.NoContent() : Results.NotFound();
    }
}