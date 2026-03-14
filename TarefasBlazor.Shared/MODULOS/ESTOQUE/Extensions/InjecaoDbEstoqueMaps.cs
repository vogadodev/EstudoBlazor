using Microsoft.EntityFrameworkCore;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.Maps;

namespace TarefasBlazor.Shared.MODULOS.ESTOQUE.Extensions
{
    public static class InjecaoDbEstoqueMaps
    {
        public static void AddEntidadesMapsEstoque(ModelBuilder modelBuilder )
        {
            modelBuilder.ApplyConfiguration(new ProdutoMap());
            modelBuilder.ApplyConfiguration(new ArmazemMap());
            modelBuilder.ApplyConfiguration(new AvaliacaoMap());
            modelBuilder.ApplyConfiguration(new CategoriaMap());
            modelBuilder.ApplyConfiguration(new EstoqueArmazemMap());
            modelBuilder.ApplyConfiguration(new FornecedorMap());
            modelBuilder.ApplyConfiguration(new MarcaMap());
            modelBuilder.ApplyConfiguration(new ProdutoEspecificacaoMap());
            modelBuilder.ApplyConfiguration(new ProdutoImagemMap());
        }
        public static void AddDTOsMapsEstoque(ModelBuilder modelBuilder)
        {

        }
    }
}
