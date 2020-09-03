using System;
using System.Data;
using System.Threading.Tasks;
using Continental.API.Core.Entities;
using Continental.API.Core.Interfaces;
using Continental.API.Infrastructure.DatabaseHelpers;
using Continental.API.Infrastructure.Settings;
using Continental.API.Infrastructure.Settings.DataBase;
using Dapper;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;

namespace Continental.API.Infrastructure.Repositories
{
    public class DapperFechasRepository : IFechasRepository
    {
        private readonly ConexionBD _conexiones;

        private readonly string _connectionStringGenerico;

        private readonly string _connectionStringConsulta;

        public DapperFechasRepository(IOptions<Configuraciones> configuraciones)
        {
            _conexiones = new ConexionBD(configuraciones.Value.SeteosBD);

            _connectionStringGenerico = _conexiones.GetCadenaDeConexion(TiposCredenciales.GENERICO, TiposDataSource.DATOSITA);

            _connectionStringConsulta = _conexiones.GetCadenaDeConexion(TiposCredenciales.SERVICIOS_CONSULTA, TiposDataSource.DATOSITA);
        }

        public async Task<bool> EsDiaHabil(DateTime fecha, CredencialesFinansys credenciales = null)
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
