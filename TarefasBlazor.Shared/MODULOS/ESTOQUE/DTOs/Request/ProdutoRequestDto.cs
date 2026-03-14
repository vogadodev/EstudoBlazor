using TarefasBlazor.Shared.MODULOS.ESTOQUE.DTOs.Response;

namespace TarefasBlazor.Shared.MODULOS.ESTOQUE.DTOs.Request
{
    public record ProdutoRequestDto
    {
        public Guid? Id { get; set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public string? CodigoUnico { get; set; }
        public decimal Preco { get; set; }
        public decimal? PrecoPromocional { get; set; }
        public bool EstaEmPromocao { get; set; }
        public bool TemFreteGratis { get; set; }
        public int? QuantidadeEstoque { get; set; }
        public bool EstaAtivo { get; set; } = true;
        public Guid MarcaId { get; set; }
        public Guid CategoriaId { get; set; }
        public Guid FornecedorId { get; set; }
        public List<ProdutoEspecificacaoDto> Especificacoes { get; set; } = new List<ProdutoEspecificacaoDto>();
        public List<ProdutoImagemDto> Imagens { get; set; } = new List<ProdutoImagemDto>();
    }
}
