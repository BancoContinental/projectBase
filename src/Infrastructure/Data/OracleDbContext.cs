using Microsoft.EntityFrameworkCore;

namespace Continental.API.Infrastructure.Data
{
    public class OracleOracleDbContext : BaseOracleDbContext<OracleOracleDbContext>
    {
        public OracleOracleDbContext(string connectionString) : base(connectionString)
        {
        }

        public OracleOracleDbContext(DbContextOptions<OracleOracleDbContext> options) : base(options)
        {
        }
    }
}