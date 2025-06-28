using DevIO.OrderProducts.Application.Commands.Produto;
using FluentValidation;

namespace DevIO.OrderProducts.Application.Validations.Produto;

public class UpdateProdutoValidator : AbstractValidator<UpdateProdutoCommand>
{
    public UpdateProdutoValidator()
    {
        RuleFor(p => p.Id)
            .NotEmpty().WithMessage("O ID do produto é obrigatório.");
        RuleFor(p => p.Nome)
            .NotEmpty().WithMessage("O nome do produto é obrigatório.")
            .Length(2, 100).WithMessage("O nome do produto deve ter entre 2 e 100 caracteres.");
        RuleFor(p => p.Preco)
            .GreaterThan(0).WithMessage("O preço do produto deve ser maior que zero.");
        RuleFor(p => p.Estoque)
            .GreaterThanOrEqualTo(0).WithMessage("Saldo em estoque do Produto deve ser positivo.");
    }
}
