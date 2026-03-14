using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.Entidades;

namespace TarefasBlazor.Shared.MODULOS.ESTOQUE.Maps
{
    public class AvaliacaoMap : IEntityTypeConfiguration<Avaliacao>
    {
        public void Configure(EntityTypeBuilder<Avaliacao> builder)
        {
            builder.ToTable("AVA_AVALIACAO_AVA");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("AVA_ID").IsRequired();
            builder.Property(x => x.ProdutoId).HasColumnName("AVA_IDPRODUTO").IsRequired();
            builder.Property(x => x.NomeAutor).HasColumnName("AVA_NOMEAUTOR").HasMaxLength(100).IsRequired();
            builder.Property(x => x.Nota).HasColumnName("AVA_NOTA").IsRequired();
            builder.Property(x => x.Comentario).HasColumnName("AVA_COMENTARIO").HasColumnType("text");
            builder.Property(x => x.DataEnvio).HasColumnName("AVA_DATAENVIO").IsRequired();
        }
    }
}