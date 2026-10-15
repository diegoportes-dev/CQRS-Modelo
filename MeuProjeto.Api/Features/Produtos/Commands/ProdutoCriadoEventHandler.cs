using MediatR;
using MongoDB.Driver;

public class ProdutoCriadoEventHandler : INotificationHandler<ProdutoCriadoEvent>
{
    private readonly IMongoCollection<ProdutoDocumento> _mongoCollection;

    public ProdutoCriadoEventHandler(IMongoDatabase mongoDatabase)
    {
        _mongoCollection = mongoDatabase.GetCollection<ProdutoDocumento>("cqrs_produto_leitura");
    }

    public async Task Handle(ProdutoCriadoEvent notification, CancellationToken cancellationToken)
    {
        var documento = new ProdutoDocumento
        {
            Id = notification.Id,
            Nome = notification.Nome,
            Preco = notification.Preco,
            Categoria = notification.Categoria,
            DataSincronizacao = DateTime.UtcNow
        };

        await _mongoCollection.InsertOneAsync(documento, cancellationToken: cancellationToken);
    }
}
