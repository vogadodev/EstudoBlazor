namespace TarefasBlazor.Shared.MODULOS.ESTOQUE.DTOs.Request
{
    public record FornecedorRequestDto(Guid? Id, string RazaoSocial, string NomeContato, string Telefone, string NomeFantasia, string CNPJ, string Email);
}
