using Continental.API.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Continental.API.Infrastructure.Data
{
    public abstract class BaseOracleDbContext<T> : DbContext where T : DbContext
    {
        private readonly string _connectionString;

        protected BaseOracleDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected BaseOracleDbContext(DbContextOptions<T> options) : base(options)
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
