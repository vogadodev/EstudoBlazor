namespace TarefasBlazor.Shared.MODULOS.ESTOQUE.DTOs.Response
{
    public record FornecedorResponseDto(
        Guid? Id,
        string RazaoSocial,
        string NomeContato,
        string Telefone,
        string NomeFantasia,
        string CNPJ,
        string Email
        );
}
