using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.Entidades;

namespace TarefasBlazor.Shared.MODULOS.ESTOQUE.Maps
{
    public class EstoqueArmazemMap : IEntityTypeConfiguration<EstoqueArmazem>
    {
        public void Configure(EntityTypeBuilder<EstoqueArmazem> builder)
        {
            builder.ToTable("AVA_ESTOQUEARMAZEM_EAR");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("EAR_ID").IsRequired();
            builder.Property(x => x.ProdutoId).HasColumnName("EAR_IDPRODUTO").IsRequired();
            builder.Property(x => x.ArmazemId).HasColumnName("EAR_IDARMAZEM").IsRequired();
            builder.Property(x => x.Quantidade).HasColumnName("EAR_QUANTIDADE").IsRequired();
            builder.Property(x => x.UltimaAtualizacao).HasColumnName("EAR_ULTIMAATUALIZACAO").IsRequired();

            #region Relacionamentos
            builder.HasOne(x => x.Produto).WithMany(p => p.EstoquesPorArmazem).HasForeignKey(x => x.ProdutoId);
            builder.HasOne(x => x.Armazem).WithMany(a => a.Estoques).HasForeignKey(x => x.ArmazemId);
            #endregion
        }
    }
}
