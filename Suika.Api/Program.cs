using FluentValidation;
using Suika.Api.Auth;
using Suika.Api.Data;
using Suika.Api.Endpoints;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(ApiKeySchemeConstants.SchemeName)
    .AddScheme<ApiKeyAuthSchemeOptions, ApiKeyAuthHandler>(ApiKeySchemeConstants.SchemeName, _ => { });
builder.Services.AddAuthorization();

// add swagger
builder.Services.AddEndpointsApiExplorer()
    .AddSwaggerGen();

// add custom endpoints
builder.Services.AddUserEndpoints()
    .AddBookEndpoints()
    .AddUserBookEndpoints();

// add validators
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// add DB and initializer
builder.Services.AddSingleton<IDbConnectionFactory>(_ => new SqliteConnectionFactory(
    builder.Configuration.GetValue<string>("Database:ConnectionString")
));
builder.Services.AddSingleton<DatabaseInitializer>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.UseUserEndpoints()
    .UseBookEndpoints()
    .UseUserBookEndpoints();

var databaseInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
await databaseInitializer.InitializeAsync();

app.Run();
