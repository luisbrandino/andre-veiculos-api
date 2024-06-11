using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AndreVeiculos.Bank.Data;
using MongoServices;
using Microsoft.Extensions.Options;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AndreVeiculosBankContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AndreVeiculosBankContext") ?? throw new InvalidOperationException("Connection string 'AndreVeiculosBankContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var settings = builder.Configuration.GetSection(nameof(MongoDatabaseSettings));
builder.Services.Configure<MongoDatabaseSettings>(settings);
builder.Services.AddSingleton<IMongoDatabaseSettings>(provider => provider.GetRequiredService<IOptions<MongoDatabaseSettings>>().Value);

builder.Services.AddSingleton<BankService>();



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
