namespace TarefasBlazor.Shared.MODULOS.COMUM.Entidades
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public List<string> Mensagens { get; set; } = new();
    }
}
