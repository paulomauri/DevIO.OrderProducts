using DevIO.OrderProducts.Application.Commands.Pedido;
using FluentValidation;


namespace DevIO.OrderProducts.Application.Validations.Pedido;

public class CreatePedidoValidator : AbstractValidator<CreatePedidoCommand>
{
    public CreatePedidoValidator()
    {

        RuleFor(x => x.Itens)
            .NotEmpty()
            .WithMessage("O pedido deve conter pelo menos um item.");

        RuleForEach(x => x.Itens)
            .ChildRules(item =>
            {
                item.RuleFor(i => i.ProdutoId)
                    .NotEmpty().WithMessage("O ID do produto é obrigatório.");
                item.RuleFor(i => i.Observacao).Must(IsValidObservacao);
                item.RuleFor(i => i.Quantidade)
                    .GreaterThan(0).WithMessage("A quantidade deve ser maior que zero.");
                item.RuleFor(i => i.PrecoUnitario)
                    .GreaterThan(0).WithMessage("O preço unitário deve ser maior que zero.");
            });
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
