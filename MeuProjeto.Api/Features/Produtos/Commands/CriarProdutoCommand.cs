using MediatR;

public record CriarProdutoCommand(string Nome, decimal Preco, Guid CategoriaId) : IRequest<Guid>;