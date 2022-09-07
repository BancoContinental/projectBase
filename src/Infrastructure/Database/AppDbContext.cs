using System;
using Continental.API.Infrastructure.Database.Helpers;

namespace Continental.API.Infrastructure.Database;

public interface IDbUnitOfWork
{
    IApplicationDbContext OracleDbContext { get; }
    IApplicationDbContext ActiveDbContext { get; }
}

public class AppDb : IDbUnitOfWork
{
    public IApplicationDbContext OracleDbContext { get; }
    public IApplicationDbContext ActiveDbContext { get; }

    public AppDb(Func<TipoConexion, IApplicationDbContext> dbContextResolverDelegate)
    {
        OracleDbContext  = dbContextResolverDelegate(TipoConexion.Oracle);
        ActiveDbContext = dbContextResolverDelegate(TipoConexion.Active);
    }
}