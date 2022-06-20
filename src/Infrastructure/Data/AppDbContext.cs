namespace Continental.API.Infrastructure.Data;

public interface IAppDbContext
{
    IOracleCommandsDbContext CommandsDbContext { get; set; }
    IOracleQueriesDbContext QueriesDbContext { get; set; }
}

public class AppDbContext : IAppDbContext
{
    public IOracleCommandsDbContext CommandsDbContext { get; set; }
    public IOracleQueriesDbContext QueriesDbContext { get; set; }
}
