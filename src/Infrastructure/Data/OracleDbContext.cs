using Continental.API.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Continental.API.Infrastructure.Data
{
    public class OracleOracleDbContext : DbContext
    {
        private readonly string _connectionString;

        public OracleOracleDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public OracleOracleDbContext(DbContextOptions<OracleOracleDbContext> options) : base(options)
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
                entity.HasKey(p => p.FechaFeriado);
                entity.Property(e => e.FechaFeriado).HasColumnName("FER_FECHA");
            });
        }
    }
}