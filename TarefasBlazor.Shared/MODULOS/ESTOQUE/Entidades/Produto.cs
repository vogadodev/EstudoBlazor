namespace TarefasBlazor.Shared.MODULOS.ESTOQUE.Entidades
{
    public class Produto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty!;
        public string Descricao { get; set; } = string.Empty!;      
        public string CodigoUnico { get; set; } 
        public decimal Preco { get; set; }
        public decimal? PrecoPromocional { get; set; }
        public bool EstaEmPromocao { get; set; }
        public bool TemFreteGratis { get; set; }
        public int QuantidadeEstoque { get; set; } 
        public bool EstaAtivo { get; set; }

        // Chaves Estrangeiras
        public Guid MarcaId { get; set; }
        public Guid CategoriaId { get; set; }
        public Guid? FornecedorId { get; set; }

        // Propriedades de Navegação
        public Marca? Marca { get; set; }
        public Categoria? Categoria { get; set; }
        public Fornecedor Fornecedor { get; set; }
        public ICollection<Avaliacao> Avaliacoes { get; set; } = new List<Avaliacao>();
        public ICollection<ProdutoImagem> Imagens { get; set; } = new List<ProdutoImagem>();
        public ICollection<ProdutoEspecificacao> Especificacoes { get; set; } = new List<ProdutoEspecificacao>();
        public ICollection<EstoqueArmazem> EstoquesPorArmazem { get; set; } = new List<EstoqueArmazem>();
    }
}
