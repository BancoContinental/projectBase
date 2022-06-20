using Continental.API.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Continental.API.Infrastructure.Data;

public interface IOracleQueriesDbContext
{
    DbSet<Feriado> Feriados { get; set; }
}

/// <summary>
/// DbContext para lecturas de la base. Debe usar conexion a ACTIVE
/// </summary>
public class OracleQueriesDbContext : DbContext, IOracleQueriesDbContext
{
    public OracleQueriesDbContext(DbContextOptions<OracleQueriesDbContext> options) : base(options)
    {
    }

    public DbSet<Feriado> Feriados { get; set; }

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
