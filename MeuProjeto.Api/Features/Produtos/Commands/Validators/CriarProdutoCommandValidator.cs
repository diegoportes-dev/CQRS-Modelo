using FluentValidation;
using Microsoft.EntityFrameworkCore;

public class CriarProdutoCommandValidator : AbstractValidator<CriarProdutoCommand>
{
    public CriarProdutoCommandValidator(SqlDbContext dbContext)
    {
        RuleFor(p => p.Nome)
            .NotEmpty().WithMessage("O nome do produto é obrigatório.")
            .MaximumLength(150).WithMessage("O nome do produto deve ter no máximo 150 caracteres.");

        RuleFor(p => p.Preco)
            .GreaterThan(0).WithMessage("O preço do produto deve ser maior que zero.");

        RuleFor(p => p.CategoriaId)
            .NotEmpty().WithMessage("A categoria do produto é obrigatória.")
            .MustAsync((categoriaId, cancellationToken) =>
                dbContext.Categorias.AnyAsync(c => c.Id == categoriaId, cancellationToken))
            .WithMessage("A categoria informada não existe.");
    }
}
