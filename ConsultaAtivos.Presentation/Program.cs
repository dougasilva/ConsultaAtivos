using ConsultaAtivos.Domain.Interfaces;
using ConsultaAtivos.Infra.Providers;
using ConsultaAtivos.Application.Services;
using ConsultaAtivos.Domain.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<IHistoricalQuoteProvider, AlphaVantageProvider>(client =>
{
    client.BaseAddress = new Uri("https://www.alphavantage.co/");
});

// Configura o binding de options
builder.Services.Configure<AlphaVantageSettings>(
    builder.Configuration.GetSection("AlphaVantage"));

builder.Services.AddHttpClient<IHistoricalQuoteProvider, AlphaVantageProvider>();

builder.Services.Configure<BrapiSettings>(builder.Configuration.GetSection("Brapi"));
builder.Services.AddHttpClient<IHistoricalQuoteProvider, BrapiProvider>();


// 👉 REGISTRA O SERVICE DO APPLICATION
builder.Services.AddScoped<ConsultaHistoricalQuoteService>();

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
