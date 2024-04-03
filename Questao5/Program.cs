using MediatR;
using Questao5.Application.Handlers;
using Questao5.Infrastructure.Database.CommandStore;
using Questao5.Infrastructure.Database.Interfaces;
using Questao5.Infrastructure.Database.QueryStore; // Certifique-se de que ContaCorrenteQueryStore está neste namespace
using Questao5.Infrastructure.Sqlite;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

// sqlite
builder.Services.AddSingleton(new DatabaseConfig { Name = builder.Configuration.GetValue<string>("DatabaseName", "Data Source=database.sqlite") });
builder.Services.AddSingleton<IDatabaseBootstrap, DatabaseBootstrap>();

// Registra as interfaces e suas implementações
builder.Services.AddScoped<IContaCorrenteQueryStore, ContaCorrenteQueryStore>();
builder.Services.AddScoped<IMovimentoCommandStore, MovimentoCommandStore>();
builder.Services.AddScoped<IIdempotenciaQueryStore, IdempotenciaQueryStore>();

// Registra o manipulador diretamente
builder.Services.AddScoped<CreateMovimentoHandler>();

builder.Services.AddTransient<IIdempotenciaQueryStore, IdempotenciaQueryStore>();
builder.Services.AddTransient<IMovimentoCommandStore, MovimentoCommandStore>();
builder.Services.AddTransient<IContaCorrenteQueryStore, ContaCorrenteQueryStore>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// sqlite
#pragma warning disable CS8602 // Desreferência de uma possível referência nula.
app.Services.GetService<IDatabaseBootstrap>().Setup();
#pragma warning restore CS8602 // Desreferência de uma possível referência nula.

app.Run();
