using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Continental.API.Core.Entities;
using Continental.API.Core.Interfaces;
using Continental.API.Infrastructure.Data;
using Continental.API.Infrastructure.DatabaseHelpers;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace Continental.API.Infrastructure.Repositories;

public class FechasRepository : IFechasRepository
{
    private readonly OracleDbContext _db;
    private readonly string _connectionStringConsulta;

    public FechasRepository(OracleDbContext db, IConfiguration configuration)
    {
        _db = db;
        _connectionStringConsulta = configuration.GetConnectionString("Oracle");
    }

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
        => await _db.Feriados.Where(e => e.Fecha == fecha.ToDateTime(TimeOnly.MinValue)).ToListAsync();
}