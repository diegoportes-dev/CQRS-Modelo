public class Produto
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = null!;
    public decimal Preco { get; private set; }
    public Guid CategoriaId { get; private set; }
    public Categoria Categoria { get; private set; } = null!;

    public Produto() { } 

    public Produto(Guid id, string nome, decimal preco, Guid categoriaId)
    {
        if (string.IsNullOrWhiteSpace(nome)) throw new ArgumentException("Nome é obrigatório.");
        if (preco <= 0) throw new ArgumentException("Preço deve ser maior que zero.");
        
        Id = id;
        Nome = nome;
        Preco = preco;
        CategoriaId = categoriaId;
    }
}