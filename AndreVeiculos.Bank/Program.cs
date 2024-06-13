using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AndreVeiculos.Bank.Data;
using MongoServices;
using Microsoft.Extensions.Options;
using AndreVeiculos.Bank.MessageProcessors;
using MessageQueueServices.Messages;
using MessageQueueServices.Consumers;
using MessageQueueServices.Settings;
using MessageQueueServices.Producers;
using MessageQueueServices.Abstractions;
using Repositories;
using Models;

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

builder.Services.AddSingleton(_ => new RabbitMqSettings() { QueueName = "message2" });

builder.Services.AddTransient<IMessage, RabbitMqMessage>();
builder.Services.AddTransient<IProducer<RabbitMqMessage>, RabbitMqProducer>();
builder.Services.AddTransient<IConsumer<RabbitMqMessage>, RabbitMqConsumer>();

builder.Services.AddTransient<RabbitMqProducer>();
builder.Services.AddTransient<RabbitMqConsumer>();

builder.Services.AddTransient<IBaseRepository<Bank>>(_ => new DapperRepository<Bank>());

builder.Services.AddHostedService<BankInsertMessageProcessor<RabbitMqMessage>>();


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
