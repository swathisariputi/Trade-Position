
using Trade_Position.Interfaces;
using Trade_Position.Services;
using Trade_Position.Repositories;
using Asp.Versioning;

var builder = WebApplication.CreateBuilder(args);

// Add controllers ONLY (no views)
builder.Services.AddControllers();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
    {
        Title = "Trade-Position API Service",
        Version = "v1"
    });
});

builder.Services.AddSingleton<ITradeRepository, TradeRepository>();
builder.Services.AddSingleton<ITradeService, TradeService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1", "Trade-Position API Service V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
