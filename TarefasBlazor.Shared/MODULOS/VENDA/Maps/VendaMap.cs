using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarefasBlazor.Shared.MODULOS.VENDA.Entidades;

namespace TarefasBlazor.Shared.MODULOS.VENDA.Maps
{
    public class VendaMap : IEntityTypeConfiguration<Venda>
    {
        public void Configure(EntityTypeBuilder<Venda> builder)
        {
            builder.ToTable("AVA_VENDA_VEN");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("VEN_ID").IsRequired();
            builder.Property(x => x.ClienteId).HasColumnName("VEN_IDCLIENTE");
            builder.Property(x => x.DataCriacao).HasColumnName("VEN_DATACRIACAO").IsRequired();
            builder.Property(x => x.ValorTotal).HasColumnName("VEN_VALORTOTAL").HasColumnType("decimal(18,2)");
            builder.Property(x => x.DataAtualizacao).HasColumnName("VEN_DATAATUALIZACAO");
            builder.Property(x => x.StatusVenda).HasColumnName("VEN_STATUSVENDA").HasConversion<int>();            
            builder.Property(x => x.StatusPagamento).HasColumnName("VEN_STATUSPAGAMENTO").HasConversion<int>();            
            builder.Property(x => x.EstaAtivo).HasColumnName("VEN_ESTAATIVO").IsRequired();

            #region Relacionamentos
            builder.HasMany(x => x.ItensVenda)
                .WithOne(i => i.Venda)
                .HasForeignKey(i => i.VendaId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion
        }
    }
}
