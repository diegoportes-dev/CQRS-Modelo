using MediatR;

public record CriarCategoriaCommand(string Descricao) : IRequest<Guid>;
