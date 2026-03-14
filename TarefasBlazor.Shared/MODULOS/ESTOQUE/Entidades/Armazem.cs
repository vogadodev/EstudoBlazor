namespace TarefasBlazor.Shared.MODULOS.ESTOQUE.Entidades
{
    public class Armazem
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty!;
        public string Localizacao { get; set; } = string.Empty!;
        public ICollection<EstoqueArmazem> Estoques { get; set; } = new List<EstoqueArmazem>();
    }
}
