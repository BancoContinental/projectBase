using Microsoft.EntityFrameworkCore;

namespace Continental.API.Infrastructure.Data
{
    public class TresLetrasOracleDbContext : BaseOracleDbContext<TresLetrasOracleDbContext>
    {
        public TresLetrasOracleDbContext(string connectionString) : base(connectionString)
        {
        }

        public TresLetrasOracleDbContext(DbContextOptions<TresLetrasOracleDbContext> options) : base(options)
        {
        }
    }
}