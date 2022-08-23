using System;
using System.Threading;
using System.Threading.Tasks;
using Continental.API.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Continental.API.Infrastructure.Database;

/// <summary>
/// DbContext para lecturas de la base. Debe usar conexion a ACTIVE
/// </summary>
public sealed class ActiveDbContext : DbContext, IApplicationDbContext
{
    private static readonly ApplicationException ReadOnlyDatabaseException = new("Base de datos de solo lectura!");

    public ActiveDbContext(DbContextOptions<ActiveDbContext> options) : base(options)
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

    public override int SaveChanges() => throw ReadOnlyDatabaseException;

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken()) => throw ReadOnlyDatabaseException;
}
