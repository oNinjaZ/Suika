using Suika.Api.Models;
using Suika.Api.Services;

namespace Suika.Api.Endpoints;

public static class UserBookEndpoints
{
    public static IServiceCollection AddUserBookEndpoints(this IServiceCollection services)
    {
        services.AddSingleton<IUserBookService, UserBookService>();
        return services;
    }

    public static IEndpointRouteBuilder UseUserBookEndpoints(this IEndpointRouteBuilder app)
    {
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

        return app;
    }
}