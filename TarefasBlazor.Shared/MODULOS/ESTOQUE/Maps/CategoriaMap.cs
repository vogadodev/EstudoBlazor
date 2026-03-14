using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.Entidades;

namespace TarefasBlazor.Shared.MODULOS.ESTOQUE.Maps
{
    public class CategoriaMap : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("AVA_CATEGORIA_CAT");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("CAT_ID").IsRequired();
            builder.Property(x => x.Nome).HasColumnName("CAT_NOME").HasMaxLength(100).IsRequired();
            builder.Property(x => x.Descricao).HasColumnName("CAT_DESCRICAO").HasMaxLength(500);
            builder.Property(x => x.CategoriaPaiId).HasColumnName("CAT_IDCATEGORIAPAI");

            #region Relacionamentos
            builder.HasOne(x => x.CategoriaPai).WithMany(c => c.Subcategorias).HasForeignKey(x => x.CategoriaPaiId);
            #endregion
        }
    }
}
