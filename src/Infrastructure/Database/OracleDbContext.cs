using System.Reflection;
using Continental.API.Core.Contracts.Data;
using Microsoft.EntityFrameworkCore;

namespace Continental.API.Infrastructure.Database;

/// <summary>
/// Db Context para inserts, updates, deletes
/// </summary>
public sealed class OracleDbContext : DbContext, IApplicationDbContext
{
    public OracleDbContext(DbContextOptions<OracleDbContext> options) : base(options)
    {
    }

    public DbSet<CuentaCorrienteDto> CuentasCorrientes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
