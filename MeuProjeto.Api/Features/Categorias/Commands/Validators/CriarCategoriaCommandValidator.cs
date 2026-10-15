using FluentValidation;

public sealed class CriarCategoriaCommandValidator : AbstractValidator<CriarCategoriaCommand>
{
    public CriarCategoriaCommandValidator()
    {
        RuleFor(command => command.Descricao)
            .NotEmpty().WithMessage("A descrição da categoria é obrigatória.");
    }
}
