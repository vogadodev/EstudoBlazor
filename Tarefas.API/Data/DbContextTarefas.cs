using Microsoft.EntityFrameworkCore;
using TarefasBlazor.Shared.Data;

namespace Tarefas.API.Data
{
    public class DbContextTarefas : BasisDbContextComum<DbContextTarefas>
    {

        public DbContextTarefas(DbContextOptions<DbContextTarefas> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {   
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
