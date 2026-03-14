using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.Entidades;

namespace TarefasBlazor.Shared.MODULOS.ESTOQUE.Maps
{
    public class FornecedorMap : IEntityTypeConfiguration<Fornecedor>
    {
        public void Configure(EntityTypeBuilder<Fornecedor> builder)
        {
            builder.ToTable("AVA_FORNECEDOR_FOR");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("FOR_ID").IsRequired();
            builder.Property(x => x.RazaoSocial).HasColumnName("FOR_NOME").HasMaxLength(200).IsRequired();
            builder.Property(x => x.CNPJ).HasColumnName("FOR_CNPJ").HasMaxLength(18).IsRequired();
            builder.Property(x => x.Email).HasColumnName("FOR_EMAIL").HasMaxLength(200).IsRequired();
            builder.Property(x => x.NomeContato).HasColumnName("FOR_NOMECONTATO").HasMaxLength(100);
            builder.Property(x => x.Telefone).HasColumnName("FOR_TELEFONE").HasMaxLength(20);
            builder.Property(x => x.NomeFantasia).HasColumnName("FOR_NOMEFANTASIA").HasMaxLength(200);
        }
    }
}
