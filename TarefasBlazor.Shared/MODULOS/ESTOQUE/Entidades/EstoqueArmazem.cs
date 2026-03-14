namespace TarefasBlazor.Shared.MODULOS.ESTOQUE.Entidades
{
    public class EstoqueArmazem
    {
        public Guid Id { get; set; }
        public Guid ProdutoId { get; set; }
        public Guid ArmazemId { get; set; }
        public int Quantidade { get; set; }
        public DateTime UltimaAtualizacao { get; set; }
        public Produto Produto { get; set; }
        public Armazem Armazem { get; set; }
    }
}