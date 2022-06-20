using Microsoft.EntityFrameworkCore;

namespace Continental.API.Infrastructure.Data;

public interface IOracleCommandsDbContext
{
}

/// <summary>
/// Db Context para inserts, updates, deletes
/// </summary>
public class OracleCommandsDbContext : DbContext, IOracleCommandsDbContext
{
    public OracleCommandsDbContext(DbContextOptions<OracleCommandsDbContext> options) : base(options)
    {
    }
}