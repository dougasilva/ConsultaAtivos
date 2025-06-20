using Microsoft.EntityFrameworkCore;

namespace ConsultaAtivos.Infra.Data
{
    public class ConsultaAtivosDbContext : DbContext
    {
        public ConsultaAtivosDbContext(DbContextOptions<ConsultaAtivosDbContext> options)
            : base(options) { }

        public DbSet<Ativo> Ativos { get; set; }
        public DbSet<HistoricoCotacao> Historicos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ativo>(entity =>
            {
                entity.HasKey(a => a.Symbol);
                entity.Property(a => a.Symbol).IsRequired().HasMaxLength(10);

                entity.HasMany(a => a.Historico)
                      .WithOne(h => h.Ativo)
                      .HasForeignKey(h => h.AtivoSymbol)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<HistoricoCotacao>(entity =>
            {
                entity.HasKey(h => new { h.AtivoSymbol, h.Data });
                entity.Property(h => h.AtivoSymbol).IsRequired();
            });
        }
    }
}
