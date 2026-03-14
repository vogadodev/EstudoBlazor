namespace TarefasBlazor.Shared.MODULOS.ESTOQUE.Entidades
{
    public class ProdutoImagem
    {
        public Guid Id { get; set; }
        public Guid ProdutoId { get; set; }
        public string UrlImagem { get; set; } = string.Empty!;
        public string TextoAlternativo { get; set; } = string.Empty!;
        public int Ordem { get; set; }
        public Produto Produto { get; set; } = null!;
    }
}
