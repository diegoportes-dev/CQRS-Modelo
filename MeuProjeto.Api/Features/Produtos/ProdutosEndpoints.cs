using MediatR;

public static class ProdutosEndpoints
{
    public static void MapProdutosEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/produtos");

        // Rota que envia um Command para o MySQL
        group.MapPost("/", async (CriarProdutoCommand command, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var id = await mediator.Send(command, cancellationToken);
            return Results.Created($"/api/produtos/{id}", new { Id = id });
        })
        .WithName("CriarProduto")
        .WithSummary("Cria um produto")
        .WithDescription("Persiste um produto no banco de escrita e retorna sua identificação.")
        .Produces(StatusCodes.Status201Created)
        .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        // Rota que envia uma Query para o MongoDB
        group.MapGet("/{id:guid}", async (Guid id, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var produto = await mediator.Send(new ObterProdutoPorIdQuery(id), cancellationToken);
            return produto is not null ? Results.Ok(produto) : Results.NotFound();
        })
        .WithName("ObterProdutoPorId")
        .WithSummary("Obtém um produto pelo identificador")
        .WithDescription("Busca os dados de leitura do produto pelo identificador.")
        .Produces<ProdutoDocumento>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
