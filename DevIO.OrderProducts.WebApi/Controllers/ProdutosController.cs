using DevIO.OrderProducts.Application.Commands.Produto;
using DevIO.OrderProducts.Application.Queries.Produto;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.OrderProducts.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase
{
    public readonly IMediator _mediator;

    public ProdutosController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var produtos = await _mediator.Send(new GetAllProdutoQuery());
        return Ok(produtos);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var produto = await _mediator.Send(new GetProdutoByIdQuery(id));
        return produto is not null ? Ok(produto) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateProdutoCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(new { Id = id, Message = "Produto criado com sucesso." });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, UpdateProdutoCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("O ID do produto não confere com o ID da requisição.");
        }

        await _mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteProdutoCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
}
