using MediatR;
using Microsoft.EntityFrameworkCore;

public class CriarProdutoCommandHandler : IRequestHandler<CriarProdutoCommand, Guid>
{
    private readonly SqlDbContext _sqlContext;
    private readonly IMediator _mediator;

    public CriarProdutoCommandHandler(SqlDbContext sqlContext, IMediator mediator)
    {
        _sqlContext = sqlContext;
        _mediator = mediator;
    }

    public async Task<Guid> Handle(CriarProdutoCommand request, CancellationToken cancellationToken)
    {
        var categoria = await _sqlContext.Categorias
            .SingleAsync(c => c.Id == request.CategoriaId, cancellationToken);
        var produto = new Produto(Guid.NewGuid(), request.Nome, request.Preco, categoria.Id);

        _sqlContext.Produtos.Add(produto);
        await _sqlContext.SaveChangesAsync(cancellationToken);

        // Dispara o evento de sincronização para o MongoDB de forma desacoplada
        await _mediator.Publish(new ProdutoCriadoEvent(produto.Id, produto.Nome, produto.Preco, categoria.Descricao), cancellationToken);

        return produto.Id;
    }
}
