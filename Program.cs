using AplicacaoAulaBackend.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DbAulaBackendContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("connection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("connection"))
    )
);

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors(o => o.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
