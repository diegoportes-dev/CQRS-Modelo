using MediatR;

public static class CategoriasEndpoints
{
    public static void MapCategoriasEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/categorias");

        group.MapPost("/", async (
            CriarCategoriaCommand command,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var id = await mediator.Send(command, cancellationToken);
            return Results.Created($"/api/categorias/{id}", new { Id = id });
        })
        .WithName("CriarCategoria")
        .WithSummary("Cria uma categoria")
        .WithDescription("Persiste uma categoria para associação com novos produtos.")
        .Produces(StatusCodes.Status201Created)
        .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        group.MapGet("/", async (IMediator mediator, CancellationToken cancellationToken) =>
            await mediator.Send(new ListarCategoriasQuery(), cancellationToken))
        .WithName("ListarCategorias")
        .WithSummary("Lista as categorias")
        .WithDescription("Retorna as categorias disponíveis para associação com produtos.")
        .Produces<IReadOnlyList<CategoriaResponse>>(StatusCodes.Status200OK);
    }
}
