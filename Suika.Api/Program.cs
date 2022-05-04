using FluentValidation;
using FluentValidation.Results;
using Suika.Api.Data;
using Suika.Api.Models;
using Suika.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddSingleton<IDbConnectionFactory>(_ => new SqliteConnectionFactory(
    builder.Configuration.GetValue<string>("Database:ConnectionString")
));
builder.Services.AddSingleton<DatabaseInitializer>();



var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/users", async (User user, IUserService userService, IValidator<User> validator) =>
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

    return Results.Created($"/users/{user.Username}", user);
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



var databaseInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
await databaseInitializer.InitializeAsync();

app.Run();
