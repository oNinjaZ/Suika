using Suika.Api.Models;
using Suika.Api.Services;

namespace Suika.Api.Endpoints;

public static class BookEndpoints
{
    public static void AddBookEndpoints(this IServiceCollection services)
    {
        services.AddSingleton<IBookService, BookService>();
    }

    public static void UseBookEndpoints(this IEndpointRouteBuilder app)
    {
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
    }
}