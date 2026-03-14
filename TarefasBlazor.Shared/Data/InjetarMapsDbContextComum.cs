using Microsoft.EntityFrameworkCore;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.Extensions;

namespace TarefasBlazor.Shared.Data
{
    public static class InjetarMapsDbContextComum
    {
        public static void AddEntitiesMapsDbContextCommon(ModelBuilder builder)
        {           
            //InjecaoDbAuthMaps.AddEntitiesMapsAuth(builder);
            InjecaoDbEstoqueMaps.AddEntidadesMapsEstoque(builder);           
            //InjecaoDbUsuarioMaps.AddEntidadesMapsUsuario(builder);
            //InjecaoDbVendasMaps.AddEntidadesMapsVendas(builder);
        }
        public static void AddDTOsMapsDbContextCommon(ModelBuilder builder)
        {           
            //InjecaoDbAuthMaps.AddDTOsMapsAuth(builder);
            InjecaoDbEstoqueMaps.AddDTOsMapsEstoque(builder);
            //InjecaoDbUsuarioMaps.AddDTOsMapsUsuario(builder);
            //InjecaoDbVendasMaps.AddDTOsMapsVendas(builder);

        }
    }
}
