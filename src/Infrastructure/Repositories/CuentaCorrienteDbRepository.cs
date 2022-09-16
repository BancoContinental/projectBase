using System.Linq;
using System.Threading.Tasks;
using Continental.API.Core.Contracts.Entities;
using Continental.API.Core.Interfaces;
using Continental.API.Infrastructure.Database;
using Continental.API.Infrastructure.Database.Helpers;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;

namespace Continental.API.Infrastructure.Repositories;

public class CuentaCorrienteDbRepository : ICuentaCorrienteRepository
{
    private readonly IAppDb _appDb;
    private readonly IConnectionStringFactory _connectionStringFactory;

    public CuentaCorrienteDbRepository(IAppDb appDb, IConnectionStringFactory connectionStringFactory)
    {
        _appDb                   = appDb;
        _connectionStringFactory = connectionStringFactory;
    }

    public async Task GetConEntityFramework()
    {
        await _appDb.ActiveDbContext.CuentasCorrientes.ToListAsync();
    }

    public async Task GetConDapper()
    {
        var connectionString = _connectionStringFactory.GetConnectionString(TipoConexion.Active);

        using var connection = new OracleConnection(connectionString);

        await connection.ExecuteScalarAsync<string>("select 'Hola, mundo!' from dual");
    }

    public async Task<CuentaCorrienteDto> ObtenerPorNumeroCuenta(string cuenta)
    {
        return await _appDb.ActiveDbContext
            .CuentasCorrientes
            .Where(p
                => p.CuentaCompleta == cuenta.PadRight(14, ' '))
            .FirstOrDefaultAsync();
    }
}
