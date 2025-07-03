using DevIO.OrderProducts.Application.Commands.Pedido;
using DevIO.OrderProducts.Application.Commands.Produto;
using DevIO.OrderProducts.Application.Interfaces.Messaging;
using DevIO.OrderProducts.Application.Settings;
using DevIO.OrderProducts.Application.Validations.Pedido;
using DevIO.OrderProducts.Application.Validations.Produto;
using DevIO.OrderProducts.Infrastructure.Data;
using DevIO.OrderProducts.Infrastructure.IoC;
using DevIO.OrderProducts.Infrastructure.Messaging;
using DevIO.OrderProducts.WebApi.Middleware;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Sinks.Network;

var builder = WebApplication.CreateBuilder(args);


// Configurar Serilog
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Host.UseSerilog((ctx, lc) =>
{
    lc.Enrich.FromLogContext()
      .WriteTo.Console()
      .WriteTo.TCPSink("tcp://192.168.0.27:5044"); // envia para Logstash via TCP
});

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurações do MediatR e FluentValidation
var applicationAssembly = typeof(CreateProdutoCommand).Assembly;

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
builder.Services.AddValidatorsFromAssembly(applicationAssembly);

// appsettings.json ? CacheTtl ? CacheTtlOptions
builder.Services.Configure<CacheTtlOptions>(
    builder.Configuration.GetSection("CacheTtl"));

// Registra a camada de cache (Redis)
builder.Services.AddCachedDependencies(); 

// Registra a camada de injeção de dependência e infraestrutura
builder.Services.AddInfrastructure(builder.Configuration);

// Kafka Producer Service
builder.Services.AddSingleton<IKafkaProducerService, KafkaProducerService>();

// Configuração do Identity Server para autenticação JWT
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:5005"; // URL do Identity Server
        options.RequireHttpsMetadata = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization();

// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

// Registra o middleware de logging de requisições e respostas
app.UseMiddleware<RequestResponseLoggingMiddleware>();

// Registra o middleware de tratamento de exceções 
app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication(); // Configuração de autenticação JWT
app.UseAuthorization();

app.MapControllers();

app.Run();
