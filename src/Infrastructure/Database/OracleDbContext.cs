using System.Reflection;
using Continental.API.Core.Contracts.Entities;
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

    public DbSet<CuentaCorriente> CuentasCorrientes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
