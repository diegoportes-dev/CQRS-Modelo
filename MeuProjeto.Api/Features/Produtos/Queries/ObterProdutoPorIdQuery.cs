using MediatR;

public record ObterProdutoPorIdQuery(Guid Id) : IRequest<ProdutoDocumento?>;
