namespace TarefasBlazor.Shared.MODULOS.VENDA.Entidades
{
    public class ItemVenda
    {       
        public Guid Id { get; set; }
        public Guid VendaId { get; set; }
        public Guid ProdutoId { get; set; }
        public string? NomeProduto { get; set; }
        public int Quantidade { get; set; }

        public decimal PrecoUnitario { get; set; }     
        public bool EstaAtivo { get; set; }

        // Relacionamento
        public Venda? Venda { get; set; }
    }
}
