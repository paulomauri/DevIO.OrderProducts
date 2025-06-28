
using DevIO.OrderProducts.Application.Commands.Pedido;
using FluentValidation;

namespace DevIO.OrderProducts.Application.Validations.Pedido;

public class AtualizarStatusPedidoValidator : AbstractValidator<AtualizarStatusPedidoCommand>
{
    public AtualizarStatusPedidoValidator()
    {
        RuleFor(x => x.PedidoId).NotEmpty();

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("O status do pedido é obrigatório.")
            .Must(status => Enum.IsDefined(typeof(Domain.Enums.StatusPedido), status))
            .WithMessage("O status do pedido deve ser um valor válido.");
    }
}
