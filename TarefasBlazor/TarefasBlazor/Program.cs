using TarefasBlazor.Components;
using TarefasBlazor.Extensions;
using TarefasBlazor.Shared.INFRA.LogServices.Interfaces;
using TarefasBlazor.Shared.INFRA.LogServices.Services;
var builder = WebApplication.CreateBuilder(args);
var configurarion = builder.Configuration;
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();



builder.Services.AddHttpClient("TarefaApi",client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiUrl"]);
}).AddHttpMessageHandler<MonitoramentoHandler>();

//Adicionando services e interfaces do proprio módulo
builder.Services
    .AddServices()
    .AddRepositories();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode();    

app.Run();
