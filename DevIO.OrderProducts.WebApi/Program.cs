using DevIO.OrderProducts.Application.Commands.Pedido;
using DevIO.OrderProducts.Application.Commands.Produto;
using DevIO.OrderProducts.Application.Validations.Pedido;
using DevIO.OrderProducts.Application.Validations.Produto;
using DevIO.OrderProducts.Infrastructure.IoC;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurações do MediatR e FluentValidation
var applicationAssembly = typeof(CreateProdutoCommand).Assembly;

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
builder.Services.AddValidatorsFromAssembly(applicationAssembly);

// Registra a camada de injeção de dependência e infraestrutura
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
