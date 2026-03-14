namespace TarefasBlazor.Shared.MODULOS.ESTOQUE.Entidades
{
    public class Marca
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty!;
        public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
    }
}
