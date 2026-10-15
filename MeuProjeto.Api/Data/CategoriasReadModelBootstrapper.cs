using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

public sealed class CategoriasReadModelBootstrapper : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IMongoCollection<CategoriaDocumento> _mongoCollection;

    public CategoriasReadModelBootstrapper(
        IServiceScopeFactory serviceScopeFactory,
        IMongoDatabase mongoDatabase)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _mongoCollection = mongoDatabase.GetCollection<CategoriaDocumento>("cqrs_categoria_leitura");
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var sqlContext = scope.ServiceProvider.GetRequiredService<SqlDbContext>();
        var categorias = await sqlContext.Categorias
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        foreach (var categoria in categorias)
        {
            var documento = new CategoriaDocumento
            {
                Id = categoria.Id,
                Descricao = categoria.Descricao
            };

            await _mongoCollection.ReplaceOneAsync(
                item => item.Id == documento.Id,
                documento,
                new ReplaceOptions { IsUpsert = true },
                cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
