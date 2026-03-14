namespace TarefasBlazor.Shared.MODULOS.ESTOQUE.Entidades
{
    public class Fornecedor
    {
        public Guid Id { get; set; }
        public string RazaoSocial { get; set; } = string.Empty!;
        public string NomeContato { get; set; } = string.Empty!;
        public string Telefone { get; set; } = string.Empty!;
        public string NomeFantasia { get; set; } = string.Empty!;
        public string Email { get; set; } = string.Empty!;
        public string CNPJ { get; set; } = string.Empty!;
        public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
    }
}
