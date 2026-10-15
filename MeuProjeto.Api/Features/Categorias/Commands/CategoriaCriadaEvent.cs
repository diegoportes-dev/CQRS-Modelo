using MediatR;

public sealed record CategoriaCriadaEvent(Guid Id, string Descricao) : INotification;
