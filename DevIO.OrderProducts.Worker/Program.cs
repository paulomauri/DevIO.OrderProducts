using DevIO.OrderProducts.Worker.Consumers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<PedidoCriadoConsumerService>();
builder.Services.AddHostedService<ProdutoAtualizadoConsumerService>();
builder.Services.AddHostedService<PedidoAtualizadoConsumerService>();
builder.Services.AddHostedService<ProdutoDeletadoConsumerService>();

var host = builder.Build();

host.Run();
