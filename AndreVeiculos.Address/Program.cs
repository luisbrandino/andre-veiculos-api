using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AndreVeiculos.Address.Data;
using AndreVeiculos.Address.Database;
using Microsoft.Extensions.Options;
using AndreVeiculos.Address.MongoServices;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AndreVeiculosAddressContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AndreVeiculosAddressContext") ?? throw new InvalidOperationException("Connection string 'AndreVeiculosAddressContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var settings = builder.Configuration.GetSection(nameof(MongoDatabaseSettings));

builder.Services.Configure<MongoDatabaseSettings>(settings);
builder.Services.AddSingleton<IMongoDatabaseSettings>(provider => provider.GetRequiredService<IOptions<MongoDatabaseSettings>>().Value);

builder.Services.AddSingleton<AddressService, AddressService>();

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
