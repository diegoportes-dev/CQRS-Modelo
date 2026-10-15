using MediatR;
using MongoDB.Driver;

public class ObterProdutoPorIdQueryHandler : IRequestHandler<ObterProdutoPorIdQuery, ProdutoDocumento?>
{
    private readonly IMongoCollection<ProdutoDocumento> _mongoCollection;

    public ObterProdutoPorIdQueryHandler(IMongoDatabase mongoDatabase)
    {
        _mongoCollection = mongoDatabase.GetCollection<ProdutoDocumento>("cqrs_produto_leitura");
    }

    public async Task<ProdutoDocumento?> Handle(ObterProdutoPorIdQuery request, CancellationToken cancellationToken)
    {
        return await _mongoCollection
            .Find(p => p.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
