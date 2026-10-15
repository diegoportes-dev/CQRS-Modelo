using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("MySqlConnectionString")
    ?? throw new InvalidOperationException("A connection string 'MySqlConnectionString' não foi configurada.");

builder.Services.AddDbContext<SqlDbContext>(options => options.UseMySQL(connectionString));

builder.Services.AddMediatR(configuration =>
    configuration.RegisterServicesFromAssembly(typeof(Program).Assembly));

var mongoConnectionString = builder.Configuration.GetConnectionString("MongoConnectionString")
    ?? throw new InvalidOperationException("A connection string 'MongoConnectionString' não foi configurada.");

var mongoDatabaseName = builder.Configuration["MongoDatabaseName"]
    ?? throw new InvalidOperationException("A configuração 'MongoDatabaseName' não foi definida.");

builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoConnectionString));

builder.Services.AddSingleton(serviceProvider =>
    serviceProvider.GetRequiredService<IMongoClient>().GetDatabase(mongoDatabaseName));

builder.Services.AddHostedService<CategoriasReadModelBootstrapper>();

// Registrar todos os validadores do FluentValidation automatico
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

// Registrar o pipeline de validação do MediatR
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapProdutosEndpoints();
app.MapCategoriasEndpoints();

app.Run();
