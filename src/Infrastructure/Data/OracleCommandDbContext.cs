using Microsoft.EntityFrameworkCore;

namespace Continental.API.Infrastructure.Data;

public interface IOracleCommandDbContext
{
}

/// <summary>
/// Db Context para inserts, updates, deletes
/// </summary>
public class OracleCommandDbContext : DbContext, IOracleCommandDbContext
{
    public OracleCommandDbContext(DbContextOptions<OracleCommandDbContext> options) : base(options)
    {
    }
}