using DevIO.OrderProducts.Application.DTO;
using MediatR;
using System;

namespace DevIO.OrderProducts.Application.Queries.Produto;
public record GetProdutoByIdQuery(Guid Id) : IRequest<ProdutoDto?>;
