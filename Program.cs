
using Trade_Position.Interfaces;
using Trade_Position.Services;
using Trade_Position.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add controllers ONLY (no views)
builder.Services.AddControllers();

// Swagger (VERY GOOD for interview)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ITradeRepository, TradeRepository>();
builder.Services.AddSingleton<TradeService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
