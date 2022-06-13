using Continental.API.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Continental.API.Infrastructure.Data
{
    public class OracleDbContext : DbContext
    {
        private readonly string _connectionString;

        public OracleDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public OracleDbContext(DbContextOptions<OracleDbContext> options) : base(options)
        {
        }

        public DbSet<Feriado> Feriados { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                base.OnConfiguring(optionsBuilder);

                optionsBuilder.UseOracle(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Feriado>(entity =>
            {
                entity.ToTable("FERIADO", "WILSON1");
                entity.HasKey(p => p.Fecha);
                entity.Property(e => e.Fecha).HasColumnName("FER_FECHA");
                entity.Property(e => e.Dia).HasColumnName("FER_COMEN").IsUnicode(false);
            });
        }
    }
}