using MediatR;

public record ListarCategoriasQuery : IRequest<IReadOnlyList<CategoriaResponse>>;

public sealed record CategoriaResponse(Guid Id, string Descricao);
