using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AndreVeiculos.Car.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AndreVeiculosCarContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AndreVeiculosCarContext") ?? throw new InvalidOperationException("Connection string 'AndreVeiculosCarContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
