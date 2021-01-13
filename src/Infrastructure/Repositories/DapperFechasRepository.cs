using System;
using System.Data;
using System.Threading.Tasks;
using Continental.API.Core.Interfaces;
using Continental.API.Infrastructure.DatabaseHelpers;
using Dapper;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace Continental.API.Infrastructure.Repositories
{
    public class DapperFechasRepository : IFechasRepository
    {
        private readonly string _connectionStringConsulta;

        public DapperFechasRepository(IConfiguration configuration)
        {
            _connectionStringConsulta = configuration.GetConnectionString("ServicioConsulta");
        }

        public async Task<bool> EsDiaHabil(DateTime fecha)
        {
            using (var connection = new OracleConnection(_connectionStringConsulta))
            {
                var dyParam = new OracleDynamicParameters();
                dyParam.Add("", OracleDbType.Date, ParameterDirection.Input, fecha.Date.ToString("dd/MM/yyyy"));

                var query = "wilson1.f_esdiahabil";

                var resultado = await connection.ExecuteScalarAsync<string>(query, dyParam, commandType: CommandType.StoredProcedure);

                return resultado.ToLower().Equals("S");
            }
        }
    }
}
