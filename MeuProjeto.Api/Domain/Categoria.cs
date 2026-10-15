public class Categoria
{
    public Guid Id { get; private set; }
    public string Descricao { get; private set; } = null!;

    public ICollection<Produto> Produtos { get; private set; } = new List<Produto>();

    public Categoria() { }

    public Categoria(Guid id, string descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao))
        {
            throw new ArgumentException("Descrição é obrigatória.", nameof(descricao));
        }

        Id = id;
        Descricao = descricao;
    }
}
