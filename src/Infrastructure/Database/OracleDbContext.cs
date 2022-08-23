using Continental.API.Core.Entities;
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

    public DbSet<Feriado> Feriados { get; set; }
}