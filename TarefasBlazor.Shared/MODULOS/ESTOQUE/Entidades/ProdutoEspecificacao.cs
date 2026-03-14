namespace TarefasBlazor.Shared.MODULOS.ESTOQUE.Entidades
{
    public class ProdutoEspecificacao
    {
        public Guid Id { get; set; }
        public Guid ProdutoId { get; set; }
        public string Chave { get; set; } = string.Empty!;
        public string Valor { get; set; } = string.Empty!;
        public Produto Produto { get; set; }
    }

}
