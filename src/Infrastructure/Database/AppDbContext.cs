using System;
using Continental.API.Infrastructure.Database.Helpers;

namespace Continental.API.Infrastructure.Database;

public interface IAppDb
{
    IApplicationDbContext OracleDbContext { get; }
    IApplicationDbContext ActiveDbContext { get; }
}

public class AppAppDb : IAppDb
{
    public IApplicationDbContext OracleDbContext { get; }
    public IApplicationDbContext ActiveDbContext { get; }

    public AppAppDb(Func<TipoConexion, IApplicationDbContext> dbContextResolverDelegate)
    {
        OracleDbContext  = dbContextResolverDelegate(TipoConexion.Oracle);
        ActiveDbContext = dbContextResolverDelegate(TipoConexion.Active);
    }
}