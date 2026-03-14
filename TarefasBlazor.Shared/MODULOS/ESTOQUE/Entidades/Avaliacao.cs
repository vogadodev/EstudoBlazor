namespace TarefasBlazor.Shared.MODULOS.ESTOQUE.Entidades
{
    public class Avaliacao
    {
        public Guid Id { get; set; }
        public Guid ProdutoId { get; set; }
        public string NomeAutor { get; set; } = string.Empty!;
        public int Nota { get; set; }
        public string Comentario { get; set; } = string.Empty!;
        public DateTime DataEnvio { get; set; }
        public Produto Produto { get; set; } = null!;
    }
}