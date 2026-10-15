using MediatR;
using MongoDB.Driver;

public sealed class ListarCategoriasQueryHandler
    : IRequestHandler<ListarCategoriasQuery, IReadOnlyList<CategoriaResponse>>
{
    private readonly IMongoCollection<CategoriaDocumento> _mongoCollection;

    public ListarCategoriasQueryHandler(IMongoDatabase mongoDatabase)
    {
        _mongoCollection = mongoDatabase.GetCollection<CategoriaDocumento>("cqrs_categoria_leitura");
    }

    public async Task<IReadOnlyList<CategoriaResponse>> Handle(
        ListarCategoriasQuery request,
        CancellationToken cancellationToken)
    {
        var categorias = await _mongoCollection
            .Find(FilterDefinition<CategoriaDocumento>.Empty)
            .SortBy(categoria => categoria.Descricao)
            .ToListAsync(cancellationToken);

        return categorias
            .Select(categoria => new CategoriaResponse(categoria.Id, categoria.Descricao))
            .ToList();
    }
}
