using DevIO.OrderProducts.Application.Commands.Produto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.OrderProducts.Application.Validations.Produto;

public class DeleteProdutoValidator : AbstractValidator<DeleteProdutoCommand>
{
    public DeleteProdutoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("O ID do produto é obrigatório para exclusão.")
            .Must(BeAValidGuid)
            .WithMessage("O ID do produto deve ser um GUID válido.");
    }

    /// <summary>
    /// Valida se o ID é um GUID válido e não é o GUID vazio.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}