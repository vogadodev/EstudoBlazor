namespace TarefasBlazor.Shared.MODULOS.ESTOQUE.Entidades
{
    public class Categoria
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty!;
        public string Descricao { get; set; } = string.Empty!;      
        public Guid? CategoriaPaiId { get; set; }
        public Categoria? CategoriaPai { get; set; }
        public ICollection<Categoria> Subcategorias { get; set; } = new List<Categoria>();
        public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
    }
}
