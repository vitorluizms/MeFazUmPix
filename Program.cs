using Microsoft.EntityFrameworkCore;
using MyWallet.Config;
using MyWallet.Data;
using MyWallet.DTOs;
using MyWallet.Middlewares;
using MyWallet.Repositories;
using MyWallet.Services;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(Options =>
{
    string host = builder.Configuration["Database:Host"] ?? string.Empty;
    string port = builder.Configuration["Database:Port"] ?? string.Empty;
    string username = builder.Configuration["Database:Username"] ?? string.Empty;
    string database = builder.Configuration["Database:Database"] ?? string.Empty;
    string password = builder.Configuration["Database:Password"] ?? string.Empty;

    string connectionString = $"Host={host};Port={port};Username={username};Password={password};Database={database};MaxPoolSize=300;";
    Options.UseNpgsql(connectionString);
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<HealthService>();
builder.Services.AddScoped<KeysService>();
builder.Services.AddScoped<KeyRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<AccountRepository>();
builder.Services.AddScoped<AuthorizationMiddleware>();
builder.Services.AddScoped<PaymentProviderRepository>();
builder.Services.AddScoped<GetKeyByValueDTO>();
builder.Services.AddScoped<PaymentsService>();
builder.Services.AddScoped<PaymentsRepository>();
builder.Services.AddScoped<ConcilliationService>();

builder.Services.AddSingleton<MessageService>();

IConfigurationSection queueConfigurationSection = builder.Configuration.GetSection("QueueSettings");
builder.Services.Configure<QueueConfig>(queueConfigurationSection);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMetricServer();
app.UseHttpMetrics(options => options.AddCustomLabel("host", context => context.Request.Host.Host));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapMetrics();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.Run();
