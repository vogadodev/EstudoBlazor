namespace TarefasBlazor.Data
{
    public static class TarefaData
    {
        public static List<Tarefa> ObterTarefas()
        {
            return new List<Tarefa>()
            {
                new Tarefa { Descricao = "Fazer aulas do curso", Concluida = false, DataCriacao = DateTime.Now, DataConclusao = null },
                new Tarefa { Descricao = "Refatorar componentes Blazor para utilizar RenderMode InteractiveServer", Concluida = false, DataCriacao = DateTime.Now, DataConclusao = null },
                new Tarefa { Descricao = "Otimizar consultas LINQ no Entity Framework para evitar N+1", Concluida = false, DataCriacao = DateTime.Now, DataConclusao = null },
                new Tarefa { Descricao = "Implementar autenticação JWT no WebService legado", Concluida = false, DataCriacao = DateTime.Now, DataConclusao = null },
                new Tarefa { Descricao = "Criar Procedure de limpeza de logs no SQL Server (T-SQL)", Concluida = false, DataCriacao = DateTime.Now, DataConclusao = null },
                new Tarefa { Descricao = "Sincronizar branches pendentes no Git e resolver conflitos", Concluida = false, DataCriacao = DateTime.Now, DataConclusao = null },
                new Tarefa { Descricao = "Criar interface de Dashboard interativo com CSS Grid e Blazor", Concluida = false, DataCriacao = DateTime.Now, DataConclusao = null },
                new Tarefa { Descricao = "Migrar chamadas SOAP para REST API onde for possível", Concluida = false, DataCriacao = DateTime.Now, DataConclusao = null },
                new Tarefa { Descricao = "Configurar Injeção de Dependência para novos serviços de Backend", Concluida = false, DataCriacao = DateTime.Now, DataConclusao = null },
                new Tarefa { Descricao = "Validar performance de índices no banco de dados MySQL", Concluida = false, DataCriacao = DateTime.Now, DataConclusao = null },
                new Tarefa { Descricao = "Implementar máscaras de input em JavaScript para campos de formulário", Concluida = false, DataCriacao = DateTime.Now, DataConclusao = null },
                new Tarefa { Descricao = "Revisar backlog da Sprint na reunião de Metodologia Ágil", Concluida = false, DataCriacao = DateTime.Now, DataConclusao = null },
                new Tarefa { Descricao = "Testar responsividade da UI utilizando Media Queries (CSS3)", Concluida = false, DataCriacao = DateTime.Now, DataConclusao = null }

            };
        }
    }
}