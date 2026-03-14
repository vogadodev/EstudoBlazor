using TarefasBlazor.Shared.MODULOS.COMUM.Entidades;
using TarefasBlazor.Shared.MODULOS.COMUM.Enums;

namespace TarefasBlazor.Shared.INFRA.ServicesComum.ServicoComMensagemService
{
    public static class MensagemExtensions
    {
        public static void AdicionarErro(this List<Mensagem> lista, string mensagem, string? detalhe = null)
        {
            lista.Add(new Mensagem(mensagem, EnumTipoMensagem.Erro , detalhe));
        }
        public static void AdicionarErroSe(this List<Mensagem> lista, bool condicao, string mensagem, string? detalhe = null)
        {
            if (condicao)
            {
                lista.Add(new Mensagem(mensagem, EnumTipoMensagem.Erro, detalhe));
            }
        }

        public static void AdicionarExcecao(this List<Mensagem> lista, Exception exception)
        {
            lista.Add(new Mensagem(exception));
        }
        public static void AdicionarAviso(this List<Mensagem> lista, string mensagem, string? detalhe = null)
        {
            lista.Add(new Mensagem(mensagem, EnumTipoMensagem.Aviso, detalhe));
        }
        public static void AdicionarInformacao(this List<Mensagem> lista, string mensagem, string? detalhe = null)
        {
            lista.Add(new Mensagem(mensagem, EnumTipoMensagem.Informacao, detalhe));
        }
        public static void AdicionarNotificacao(this List<Mensagem> lista, string mensagem, string? detalhe = null)
        {
            lista.Add(new Mensagem(mensagem, EnumTipoMensagem.Notificacao, detalhe));
        }

        public static bool TemErros(this List<Mensagem> lista)
        {
            return lista.Exists(n => n.Tipo == EnumTipoMensagem.Erro);
        }
    }
}