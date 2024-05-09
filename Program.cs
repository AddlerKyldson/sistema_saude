using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using sistema_saude.Data;
using System.Text.Json.Serialization; // Importe o namespace para JsonSerializerOptions

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers() // Adiciona o serviço MVC
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve; // Adiciona suporte para referências circulares
        options.JsonSerializerOptions.WriteIndented = true; // Torna o JSON de saída formatado de forma legível
    });
builder.Services.AddDbContext<MyDbContext>(options => // Registra o DbContext
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Adicionar serviço CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowSpecificOrigin",
        builder =>
            builder
                .WithOrigins("http://localhost:3000") // Adicione outros métodos de configuração conforme necessário, como WithMethods(), WithHeaders(), etc.
                .AllowAnyMethod()
                .AllowAnyHeader()
    );
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Usar o middleware CORS
app.UseCors("AllowSpecificOrigin");

app.MapControllers(); // Usa o sistema de roteamento para controllers

app.Run();
