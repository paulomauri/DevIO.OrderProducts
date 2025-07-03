using DevIO.OrderProducts.Application.Commands.Pedido;
using DevIO.OrderProducts.Application.Queries.Pedido;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.OrderProducts.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PedidoController : ControllerBase
{
    public readonly IMediator _mediator;

    public PedidoController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var pedidos = await _mediator.Send(new GetAllPedidoQuery());
        return Ok(pedidos);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var pedido = await _mediator.Send(new GetPedidoByIdQuery(id));
        return pedido is not null ? Ok(pedido) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreatePedidoCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(new { Id = id, Message = "Pedido criado com sucesso." });
    }

    [HttpPost]
    [Route("api/[controller]/atualizastatus")]
    public async Task<IActionResult> AtualizaStatus([FromBody] AtualizarStatusPedidoCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
}
