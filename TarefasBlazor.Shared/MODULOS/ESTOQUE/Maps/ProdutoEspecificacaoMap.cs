using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.Entidades;

namespace TarefasBlazor.Shared.MODULOS.ESTOQUE.Maps
{
    public class ProdutoEspecificacaoMap : IEntityTypeConfiguration<ProdutoEspecificacao>
    {
        public void Configure(EntityTypeBuilder<ProdutoEspecificacao> builder)
        {
            builder.ToTable("AVA_PRODUTOESPECIFICACAO_PES");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("PES_ID").IsRequired();
            builder.Property(x => x.ProdutoId).HasColumnName("PES_IDPRODUTO").IsRequired();
            builder.Property(x => x.Chave).HasColumnName("PES_CHAVE").HasMaxLength(100).IsRequired();
            builder.Property(x => x.Valor).HasColumnName("PES_VALOR").HasMaxLength(255).IsRequired();
        }
    }
}
