using MediatR;

public sealed class CriarCategoriaCommandHandler : IRequestHandler<CriarCategoriaCommand, Guid>
{
    private readonly SqlDbContext _sqlContext;
    private readonly IMediator _mediator;

    public CriarCategoriaCommandHandler(SqlDbContext sqlContext, IMediator mediator)
    {
        _sqlContext = sqlContext;
        _mediator = mediator;
    }

    public async Task<Guid> Handle(CriarCategoriaCommand request, CancellationToken cancellationToken)
    {
        var categoria = new Categoria(Guid.NewGuid(), request.Descricao);

        _sqlContext.Categorias.Add(categoria);
        await _sqlContext.SaveChangesAsync(cancellationToken);

        await _mediator.Publish(
            new CategoriaCriadaEvent(categoria.Id, categoria.Descricao),
            cancellationToken);

        return categoria.Id;
    }
}
