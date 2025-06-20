using ConsultaAtivos.Application.Interfaces;
using ConsultaAtivos.Application.Mapping;
using ConsultaAtivos.Application.Services;
using ConsultaAtivos.Domain.Configuration;
using ConsultaAtivos.Domain.Interfaces;
using ConsultaAtivos.Infra.Data;
using ConsultaAtivos.Infra.Repositories;
using ConsultaAtivos.Infra.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add controllers e swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Configurações da Brapi
builder.Services.Configure<BrapiSettings>(builder.Configuration.GetSection("BrapiSettings"));
builder.Services.AddHttpClient<IConsultaAtivosService, ConsultaAtivosService>();
builder.Services.AddHttpClient<IBrapiService, BrapiService>();

builder.Services.AddDbContext<ConsultaAtivosDbContext>(options =>
    options.UseSqlite("Data Source=consultaativos.db"));

builder.Services.AddScoped<IAtivoRepository, AtivoRepository>();
builder.Services.AddScoped<IHistoricoCotacaoRepository, HistoricoCotacaoRepository>();

// Registro do AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

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
