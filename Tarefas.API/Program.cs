using Tarefas.API.Data;
using Tarefas.API.InjecaoDependencias;
using TarefasBlazor.Shared.INFRA;
using TarefasBlazor.Shared.MODULOS.LOG.Entidades;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
const string DbConnectionName = "TarefasDbConnection";
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddRepositories()
    .AddServices();
//Adicionando Serviços e DBContext
builder.Services.AddInfraServicosComum(configuration).
             AddDbContextConfiguration<DbContextTarefas>(configuration, DbConnectionName);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.AddCustomMiddlewares();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
