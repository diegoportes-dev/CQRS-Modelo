using MediatR;

public record ProdutoCriadoEvent(Guid Id, string Nome, decimal Preco, string Categoria) : INotification;
