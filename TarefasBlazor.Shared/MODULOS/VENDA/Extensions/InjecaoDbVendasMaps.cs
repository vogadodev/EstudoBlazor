using Microsoft.EntityFrameworkCore;
using TarefasBlazor.Shared.MODULOS.VENDA.Maps;

namespace TarefasBlazor.Shared.MODULOS.VENDA.Extensions
{
    public static class InjecaoDbVendasMaps
    {
        public static void AddEntidadesMapsVendas(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ItemVendaMap());
            modelBuilder.ApplyConfiguration(new VendaMap());           
        }
        public static void AddDTOsMapsVendas(ModelBuilder modelBuilder)
        {

        }
    }
}
