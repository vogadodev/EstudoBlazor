using Tarefas.API.Data;
using Tarefas.API.Services.CategoriaServices;
using Tarefas.API.Services.FronecedorServices;
using Tarefas.API.Services.MarcaServices;
using Tarefas.API.Services.ProdutoServices;
using Tarefas.API.Services.RabbitMQServices;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.Repositories;


namespace Tarefas.API.InjecaoDependencias
{
    public static class AddDependenciesServicesModule
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            //Produto
            services.AddScoped(typeof(GravarProdutoService));
            services.AddScoped(typeof(ValidarProdutoService));
            services.AddScoped(typeof(ObterProdutoService));
            services.AddScoped(typeof(ExcluirProdutoService));

            //Fornecedor
            services.AddScoped(typeof(GravarFornecedorService));
            services.AddScoped(typeof(ValidarFornecedorService));
            services.AddScoped(typeof(ObterFornecedorService));
            services.AddScoped(typeof(ExcluirFornecedorService));

            //Marca
            services.AddScoped(typeof(GravarMarcaService));
            services.AddScoped(typeof(ValidarMarcaService));
            services.AddScoped(typeof(ObterMarcaService));
            services.AddScoped(typeof(ExcluirMarcaService));

            //Categoria
            services.AddScoped(typeof(GravarCategoriaService));
            services.AddScoped(typeof(ValidarCategoriaService));
            services.AddScoped(typeof(ObterCategoriaService));
            services.AddScoped(typeof(ExcluirCategoriaService));

            //Mensageria RabbitMQ
            services.AddHostedService<ProcessarPedidoConsumer>();

            return services;

        }
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(ProdutoRepository<DbContextTarefas>));
            services.AddScoped(typeof(FornecedorRepository<DbContextTarefas>));
            services.AddScoped(typeof(MarcaRepository<DbContextTarefas>));
            services.AddScoped(typeof(CategoriaRepository<DbContextTarefas>));
            services.AddScoped(typeof(AvaliacaoRepository<DbContextTarefas>));
            return services;
        }
    }
}
