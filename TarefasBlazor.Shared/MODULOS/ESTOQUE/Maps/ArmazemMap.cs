using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.Entidades;

namespace TarefasBlazor.Shared.MODULOS.ESTOQUE.Maps
{

    public class ArmazemMap : IEntityTypeConfiguration<Armazem>
    {
        public void Configure(EntityTypeBuilder<Armazem> builder)
        {
            builder.ToTable("AVA_ARMAZEM_ARM");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ARM_ID").IsRequired();
            builder.Property(x => x.Nome).HasColumnName("ARM_NOME").HasMaxLength(100).IsRequired();
            builder.Property(x => x.Localizacao).HasColumnName("ARM_LOCALIZACAO").HasMaxLength(200);
        }
    }
}
