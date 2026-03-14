using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.Entidades;

namespace TarefasBlazor.Shared.MODULOS.ESTOQUE.Maps
{
    public class ProdutoMap : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("AVA_PRODUTO_PRO");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("PRO_ID").IsRequired();
            builder.Property(x => x.Nome).HasColumnName("PRO_NOME").HasMaxLength(200).IsRequired();
            builder.Property(x => x.Descricao).HasColumnName("PRO_DESCRICAO").HasColumnType("text");
            builder.Property(x => x.CodigoUnico).HasColumnName("PRO_SKU").HasMaxLength(50).IsRequired();
            builder.Property(x => x.Preco).HasColumnName("PRO_PRECO").HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(x => x.PrecoPromocional).HasColumnName("PRO_PRECOPROMOCIONAL").HasColumnType("decimal(18,2)");
            builder.Property(x => x.EstaEmPromocao).HasColumnName("PRO_ESTAEMPROMOCAO").IsRequired();
            builder.Property(x => x.TemFreteGratis).HasColumnName("PRO_TEMFRETEGRATIS").IsRequired();
            builder.Property(x => x.QuantidadeEstoque).HasColumnName("PRO_QUANTIDADEESTOQUE").IsRequired();
            builder.Property(x => x.EstaAtivo).HasColumnName("PRO_ESTAATIVO").IsRequired();
            builder.Property(x => x.MarcaId).HasColumnName("PRO_IDMARCA").IsRequired();
            builder.Property(x => x.CategoriaId).HasColumnName("PRO_IDCATEGORIA").IsRequired();
            builder.Property(x => x.FornecedorId).HasColumnName("PRO_IDFORNECEDOR");

            #region Relacionamentos
            builder.HasOne(x => x.Marca).WithMany(m => m.Produtos).HasForeignKey(x => x.MarcaId);
            builder.HasOne(x => x.Categoria).WithMany(c => c.Produtos).HasForeignKey(x => x.CategoriaId);
            builder.HasOne(x => x.Fornecedor).WithMany(f => f.Produtos).HasForeignKey(x => x.FornecedorId);
            builder.HasMany(x => x.Avaliacoes).WithOne(a => a.Produto).HasForeignKey(a => a.ProdutoId);
            builder.HasMany(x => x.Imagens).WithOne(i => i.Produto).HasForeignKey(i => i.ProdutoId);
            builder.HasMany(x => x.Especificacoes).WithOne(e => e.Produto).HasForeignKey(e => e.ProdutoId);
            builder.HasMany(x => x.EstoquesPorArmazem).WithOne(e => e.Produto).HasForeignKey(e => e.ProdutoId);
            #endregion
        }
    }
}
