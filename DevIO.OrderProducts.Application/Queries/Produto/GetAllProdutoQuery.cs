using DevIO.OrderProducts.Application.DTO;
using MediatR;


namespace DevIO.OrderProducts.Application.Queries.Produto;

public record GetAllProdutoQuery() : IRequest<IEnumerable<ProdutoDto>>;
