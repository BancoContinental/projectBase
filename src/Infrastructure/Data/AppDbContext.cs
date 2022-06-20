namespace Continental.API.Infrastructure.Data;

public interface IAppDbContext
{
    IOracleCommandDbContext CommandDbContext { get; set; }
    IOracleQueryDbContext QueryDbContext { get; set; }
}

public class AppDbContext : IAppDbContext
{
    public IOracleCommandDbContext CommandDbContext { get; set; }
    public IOracleQueryDbContext QueryDbContext { get; set; }
}
