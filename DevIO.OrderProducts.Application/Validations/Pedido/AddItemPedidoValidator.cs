using DevIO.OrderProducts.Application.Commands.Pedido;
using FluentValidation;

namespace DevIO.OrderProducts.Application.Validations.Pedido;

public class AddItemPedidoValidator : AbstractValidator<AddItemPedidoCommand>
{
    public AddItemPedidoValidator()
    {
        RuleFor(x => x.PedidoId)
            .NotEmpty().WithMessage("O ID do pedido é obrigatório.");
        RuleFor(x => x.ProdutoId)
            .NotEmpty().WithMessage("O ID do produto é obrigatório.");
        RuleFor(x => x.Quantidade)
            .GreaterThan(0).WithMessage("A quantidade deve ser maior que zero.");
        RuleFor(x => x.PrecoUnitario)
            .GreaterThan(0).WithMessage("O preço unitário deve ser maior que zero.");
        RuleFor(x => x.Observacao)
            .Must(IsValidObservacao).WithMessage("A observação não pode ser apenas um ponto (.) ou vazia.");
    }

    public bool IsValidObservacao(string? observacao)
    {
        // Implementar a lógica de validação da observação, se necessário.
        if (string.IsNullOrEmpty(observacao))
        {
            return true; // Se a observação for nula, consideramos válida.
        }

        if (observacao.Equals("."))
        {
            return false; // Se a observação somente conter ".", consideramos inválida.
        }

        return true; // Retornar true se a observação for válida, false caso contrário.
    }
}
