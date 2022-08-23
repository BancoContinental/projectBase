using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Continental.API.Core.Entities;
using Continental.API.Core.Interfaces;
using Continental.API.Infrastructure.Database;
using Continental.API.Infrastructure.Database.Helpers;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace Continental.API.Infrastructure.Repositories;

public class FechasSistemaReadOnlyRepository : IFechasSistemaRepository
{
    private readonly IDbUnitOfWork _db;
    private readonly string _connectionStringConsulta;

    public FechasSistemaReadOnlyRepository(IDbUnitOfWork db, IConfiguration configuration)
    {
        _db = db;
        _connectionStringConsulta = configuration.GetConnectionString("Active");
    }

    /// <summary>
    /// Esta funcion se debe aprobar por excepcion en el Pull Request.
    /// Los repositorios ReadOnly solo deben contener selects
    /// </summary>
    /// <param name="fecha">Fecha a consultar</param>
    /// <returns>True si es dia habil</returns>
    public async Task<bool> GetDiaHabil(DateTime fecha)
    {
        using (var connection = new OracleConnection(_connectionStringConsulta))
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("", OracleDbType.Date, ParameterDirection.Input, fecha.Date.ToString("dd/MM/yyyy"));

            var query = "wilson1.f_esdiahabil";

            var resultado =
                await connection.ExecuteScalarAsync<string>(query, dyParam, commandType: CommandType.StoredProcedure);

            return resultado.ToUpper().Equals("S");
        }
    }

    public async Task<List<Feriado>> GetFeriado(DateOnly fecha)
        => await _db.ActiveDbContext.Feriados
            .AsNoTracking()
            .Where(e => e.Fecha == fecha.ToDateTime(TimeOnly.MinValue))
            .ToListAsync();
}
