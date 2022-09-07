using System.Threading.Tasks;
using Continental.API.Core.Interfaces;
using Continental.API.Infrastructure.Database;
using Continental.API.Infrastructure.Database.Helpers;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;

namespace Continental.API.Infrastructure.Repositories;

public class MyRepository : IMyRepository
{
    private readonly IDbUnitOfWork _db;
    private readonly IConnectionStringFactory _connectionStringFactory;

    public MyRepository(IDbUnitOfWork db, IConnectionStringFactory connectionStringFactory)
    {
        _db                      = db;
        _connectionStringFactory = connectionStringFactory;
    }

    public async Task GetConEntityFramework()
    {
        await _db.ActiveDbContext.CuentasCorrientes.ToListAsync();
    }

    public async Task GetConDapper()
    {
        var connectionString = _connectionStringFactory.GetConnectionString(TipoConexion.Active);

        using var connection = new OracleConnection(connectionString);

        await connection.ExecuteScalarAsync<string>("select 'Hola, mundo!' from dual");
    }
}