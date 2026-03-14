using Microsoft.EntityFrameworkCore;

namespace TarefasBlazor.Shared.Data
{
    public class BasisDbContextComum<T>:DbContext  where T : DbContext
    {
        protected BasisDbContextComum(DbContextOptions options)
        : base(options) { }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            InjetarMapsDbContextComum.AddEntitiesMapsDbContextCommon(modelBuilder);
            InjetarMapsDbContextComum.AddDTOsMapsDbContextCommon(modelBuilder);           
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {         
            base.OnConfiguring(optionsBuilder);
        }
    }
}
