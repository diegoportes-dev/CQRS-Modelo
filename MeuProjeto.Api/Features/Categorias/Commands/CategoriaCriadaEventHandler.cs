using MediatR;
using MongoDB.Driver;

public sealed class CategoriaCriadaEventHandler : INotificationHandler<CategoriaCriadaEvent>
{
    private readonly IMongoCollection<CategoriaDocumento> _mongoCollection;

    public CategoriaCriadaEventHandler(IMongoDatabase mongoDatabase)
    {
        _mongoCollection = mongoDatabase.GetCollection<CategoriaDocumento>("cqrs_categoria_leitura");
    }

    public async Task Handle(CategoriaCriadaEvent notification, CancellationToken cancellationToken)
    {
        var documento = new CategoriaDocumento
        {
            Id = notification.Id,
            Descricao = notification.Descricao
        };

        await _mongoCollection.ReplaceOneAsync(
            categoria => categoria.Id == documento.Id,
            documento,
            new ReplaceOptions { IsUpsert = true },
            cancellationToken);
    }
}
