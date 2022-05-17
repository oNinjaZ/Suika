using FluentValidation;
using Suika.Api.Extensions;
using Suika.Api.Models;
using Suika.Api.Services;

namespace Suika.Api.Endpoints;

public static class UserEndpoints
{
    public static void AddUserEndpoints(this IServiceCollection services)
    {
        services.AddSingleton<IUserService, UserService>();
    }

    public static void UseUserEndpoints(this IEndpointRouteBuilder app)
    {
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
}).WithName("GetUsers");

        app.MapGet("/users/{username}", async (string username, IUserService userService) =>
        {
            var user = await userService.GetByUsernameAsync(username);
            return user is not null ? Results.Ok(user.AsDto()) : Results.NotFound();
        }).WithName("GetUser");

        app.MapPost("/users",
            //[Authorize(AuthenticationSchemes = ApiKeySchemeConstants.SchemeName)]
            async (User user, IUserService userService, IValidator<User> validator) =>
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

    //var locationUri = linker.GetUriByName(context, "GetUser", new { username = user.Username })!;
    //return Results.Created(locationUri, user.AsDto());
            return Results.CreatedAtRoute("GetUser", new { username = user.Username }, user.AsDto());
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
    }

}