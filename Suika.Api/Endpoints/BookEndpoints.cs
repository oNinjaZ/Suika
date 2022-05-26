using Suika.Api.Models;
using Suika.Api.Services;

namespace Suika.Api.Endpoints;

public static class BookEndpoints
{
    public static IServiceCollection AddBookEndpoints(this IServiceCollection services)
    {
        services.AddSingleton<IBookService, BookService>();
        return services;
    }

    public static IEndpointRouteBuilder UseBookEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/books", CreateBookAsync)
            .WithName("CreateBook")
            .Accepts<Book>("application/json");

        app.MapGet("/books", GetBookAsync)
            .WithName("GetAllBooks");

        return app;
    }

    internal static async Task<IResult> GetBookAsync(IBookService bookService)
    {
        var books = await bookService.GetAllAsync();
        return Results.Ok(books);
    }

    internal static async Task<IResult> CreateBookAsync(Book book, IBookService bookService)
    {
        var created = await bookService.CreateAsync(book);
        if (!created)
        {
            return Results.BadRequest(); // todo return validation error
        }

        return Results.Ok();
    }
}