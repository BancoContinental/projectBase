using System.Collections.Generic;
using System.Data;
using System.Linq;
using Continental.API.Seguridad.Dominio;
using Continental.API.Seguridad.Utiles;
using Dapper;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace Continental.API.Seguridad.Repositorio
{
    class TokenRepositorio
    {
        private readonly string _connectionStringSeguridad;

        private readonly string _connectionStringTemplate;

        public TokenRepositorio(IConfiguration configuration)
        {
            _connectionStringSeguridad = configuration.GetConnectionString("ServicioSeguridad");
            _connectionStringTemplate  = configuration.GetConnectionString("Template");
        }

        public string GetIdToken()
        {
            var oracleConexion = new OracleConnection(_connectionStringSeguridad);

            try
            {
                oracleConexion.Open();

                var dyParam = new DynamicParameters();
                dyParam.Add("@po_salida", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);

                var query = "servicios.sp_get_id_token_activo";

                SqlMapper.ExecuteScalar<string>(oracleConexion
                    , query
                    , param: dyParam
                    , commandType: CommandType.StoredProcedure);

                var respuesta = dyParam.Get<string>("@po_salida");

                return respuesta;
            }
            finally
            {
                oracleConexion.Close();
                oracleConexion.Dispose();
            }
        }

        public string[] GetCanalesPorToken(string idToken)
        {
            var oracleConexion = new OracleConnection(_connectionStringSeguridad);

            try
            {
                oracleConexion.Open();

                var dyParam = new OracleDynamicParameters();
                dyParam.Add("pi_id_token", OracleDbType.Varchar2, ParameterDirection.Input, idToken);
                dyParam.Add("po_salida", OracleDbType.RefCursor, ParameterDirection.Output);

                var query = "servicios.sp_get_canales_por_token";

                var result = SqlMapper.Query<string>(oracleConexion,
                        query,
                        param: dyParam,
                        commandType: CommandType.StoredProcedure)
                    .ToArray();

                return result;
            }
            finally
            {
                oracleConexion.Close();
                oracleConexion.Dispose();
            }
        }

        public string[] GetDireccionesIpPorCanal(string canal)
        {
            var oracleConexion = new OracleConnection(_connectionStringSeguridad);

            try
            {
                oracleConexion.Open();

                var dyParam = new OracleDynamicParameters();
                dyParam.Add("pi_canal", OracleDbType.Varchar2, ParameterDirection.Input, canal);
                dyParam.Add("po_salida", OracleDbType.RefCursor, ParameterDirection.Output);

                var query = "servicios.sp_get_ip_hab_por_canal";

                var result = SqlMapper
                    .Query<string>(oracleConexion
                        , query
                        , param: dyParam
                        , commandType: CommandType.StoredProcedure)
                    .ToArray();

                return result;
            }
            finally
            {
                oracleConexion.Close();
                oracleConexion.Dispose();
            }
        }

        public string GetRolPorUsuario(string usuario)
        {
            var oracleConexion = new OracleConnection(_connectionStringSeguridad);

            try
            {
                oracleConexion.Open();

                var dyParam = new DynamicParameters();
                dyParam.Add("@pi_usuario", Crypto.Decrypt(usuario, true));
                dyParam.Add("@po_salida", dbType: DbType.String, direction: ParameterDirection.Output, size: 4);

                var query = "servicios.sp_get_depart_por_usuario";

                SqlMapper.ExecuteScalar<string>(oracleConexion,
                    query,
                    param: dyParam,
                    commandType: CommandType.StoredProcedure);

                var respuesta = dyParam.Get<string>("@po_salida");

                return respuesta;
            }
            finally
            {
                oracleConexion.Close();
                oracleConexion.Dispose();
            }
        }

        public List<RecursoRol> GetRecursosHabilitados()
        {
            var oracleConexion = new OracleConnection(_connectionStringSeguridad);

            try
            {
                oracleConexion.Open();

                var dyParam = new OracleDynamicParameters();
                dyParam.Add("po_salida", OracleDbType.RefCursor, ParameterDirection.Output);

                var query = "servicios.sp_get_recursos_activos";

                var result = SqlMapper.Query<RecursoRol>(oracleConexion,
                        query,
                        param: dyParam,
                        commandType: CommandType.StoredProcedure)
                    .ToList();

                return result;
            }
            finally
            {
                oracleConexion.Close();
                oracleConexion.Dispose();
            }
        }

        public string GetSecretKeyPorIdToken(string idToken)
        {
            var oracleConexion = new OracleConnection(_connectionStringSeguridad);

            try
            {
                oracleConexion.Open();

                var dyParam = new DynamicParameters();
                dyParam.Add("@pi_id", idToken);
                dyParam.Add("@po_salida", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);

                var query = "servicios.sp_get_secretkey_por_id";

                SqlMapper
                    .ExecuteScalar<string>(oracleConexion
                        , query
                        , param: dyParam
                        , commandType: CommandType.StoredProcedure);

                var respuesta = dyParam.Get<string>("@po_salida");

                return respuesta;
            }
            finally
            {
                oracleConexion.Close();
                oracleConexion.Dispose();
            }
        }

        public bool ValidateAuthenticationFsys(string user, string pass)
        {
            var ok = false;

            var cadena = string.Format(
                _connectionStringTemplate,
                Crypto.Decrypt(user, true),
                Crypto.Decrypt(pass, true));

            var connection = new OracleConnection(cadena);

            connection.Open();

            ok = true;

            connection.Close();

            return ok;
        }

        public int IdentificarLaIpRemota(string ipRemota)
        {
            var cantidad = 0;
            var connection = new OracleConnection(_connectionStringSeguridad);

            try
            {
                connection.Open();

                var dyParam = new DynamicParameters();
                dyParam.Add("@pi_ip_remota", ipRemota);
                dyParam.Add("@po_salida", dbType: DbType.Int32, direction: ParameterDirection.Output);

                var query = "servicios.sp_verif_existencia_ip";

                SqlMapper.ExecuteScalar<int>(connection, query, param: dyParam,
                    commandType: CommandType.StoredProcedure);

                cantidad = dyParam.Get<int>("@po_salida");
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }

            return cantidad;
        }

        public int GetIdCanalPorIp(string ipRemota)
        {
            var oracleConexion = new OracleConnection(_connectionStringSeguridad);

            try
            {
                oracleConexion.Open();

                var dyParam = new DynamicParameters();
                dyParam.Add("@pi_ip_remota", ipRemota);
                dyParam.Add("@po_salida", dbType: DbType.Int32, direction: ParameterDirection.Output);

                var query = "servicios.sp_get_id_canal_por_ip";

                SqlMapper.ExecuteScalar<int>(oracleConexion,
                    query,
                    param: dyParam,
                    commandType: CommandType.StoredProcedure);

                var respuesta = dyParam
                    .Get<int>("@po_salida");

                return respuesta;
            }
            finally
            {
                oracleConexion.Close();
                oracleConexion.Dispose();
            }
        }

        public Canal GetDatosCanalPorId(string nombreCanal)
        {
            var oracleConexion = new OracleConnection(_connectionStringSeguridad);

            try
            {
                oracleConexion.Open();

                var dyParam = new OracleDynamicParameters();
                dyParam.Add("pi_nombre_canal", OracleDbType.Varchar2, ParameterDirection.Input, nombreCanal);
                dyParam.Add("po_salida", OracleDbType.RefCursor, ParameterDirection.Output);

                var query = "servicios.sp_get_canal_x_id2";

                var result = SqlMapper
                    .QueryFirstOrDefault<Canal>(oracleConexion
                        , query
                        , param: dyParam
                        , commandType: CommandType.StoredProcedure);

                return result;
            }
            finally
            {
                oracleConexion.Close();
                oracleConexion.Dispose();
            }
        }

        public string GetClienteExternoPorIp(int idCanal, string ipRemota)
        {
            var oracleConexion = new OracleConnection(_connectionStringSeguridad);

            try
            {
                oracleConexion.Open();

                var dyParam = new DynamicParameters();
                dyParam.Add("@pi_id_canal", idCanal);
                dyParam.Add("@pi_ip_remota", ipRemota);
                dyParam.Add("@po_salida", dbType: DbType.String, direction: ParameterDirection.Output, size: 15);

                var query = "servicios.sp_get_clie_por_ip";

                SqlMapper.ExecuteScalar<string>(oracleConexion,
                    query,
                    param: dyParam,
                    commandType: CommandType.StoredProcedure);

                var respuesta = dyParam.Get<string>("@po_salida");

                return respuesta;
            }
            finally
            {
                oracleConexion.Close();
                oracleConexion.Dispose();
            }
        }

        public string GetClienteExternoPorCanalIp(int idCanal, string ipRemota)
        {
            var oracleConexion = new OracleConnection(_connectionStringSeguridad);

            try
            {
                oracleConexion.Open();

                var dyParam = new DynamicParameters();
                dyParam.Add("@pi_id_canal", idCanal);
                dyParam.Add("@pi_ip_remota", ipRemota);
                dyParam.Add("@po_salida", dbType: DbType.String, direction: ParameterDirection.Output, size: 15);

                var query = "servicios.sp_get_clie_por_canal_ip";

                SqlMapper.ExecuteScalar<string>(oracleConexion,
                    query,
                    param: dyParam,
                    commandType: CommandType.StoredProcedure);

                var respuesta = dyParam.Get<string>("@po_salida");

                return respuesta;
            }
            finally
            {
                oracleConexion.Close();
                oracleConexion.Dispose();
            }
        }

        public bool AutenticacionBancaCorporativaCorrecta(Credenciales credenciales, string empresa)
        {
            var ok = false;
            var connection = new OracleConnection(_connectionStringSeguridad);

            try
            {
                connection.Open();

                var dyParam = new DynamicParameters();
                dyParam.Add("@pi_usuario", credenciales.Usuario);
                dyParam.Add("@pi_contrasenha", Crypto.Encrypt(credenciales.Password));
                dyParam.Add("@pi_empresa", empresa);
                dyParam.Add("@po_salida", dbType: DbType.String, direction: ParameterDirection.Output, size: 10);

                var query = "servicios.sp_valida_autenticacion_corp";

                SqlMapper.ExecuteScalar<int>(connection, query, param: dyParam,
                    commandType: CommandType.StoredProcedure);

                var respuesta = dyParam.Get<string>("@po_salida");

                ok = respuesta.Equals("S") ? true : false;
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }

            return ok;
        }

        public List<Canal> GetCanalesPorIp(string ipRemota)
        {
            var oracleConexion = new OracleConnection(_connectionStringSeguridad);

            try
            {
                oracleConexion.Open();

                var dyParam = new OracleDynamicParameters();
                dyParam.Add("pi_ip_remota", OracleDbType.Varchar2, ParameterDirection.Input, ipRemota);
                dyParam.Add("po_salida", OracleDbType.RefCursor, ParameterDirection.Output);

                var query = "servicios.sp_get_canales_por_ip";

                var result = SqlMapper.Query<Canal>(oracleConexion,
                        query,
                        param: dyParam,
                        commandType: CommandType.StoredProcedure)
                    .ToList();

                return result;
            }
            finally
            {
                oracleConexion.Close();
                oracleConexion.Dispose();
            }
        }

        public bool EsAutenticacionPorCanalCorrecta(Credenciales credenciales, int idCanal)
        {
            var ok = false;
            var connection = new OracleConnection(_connectionStringSeguridad);

            try
            {
                connection.Open();

                var dyParam = new DynamicParameters();
                dyParam.Add("@pi_usuario", credenciales.Usuario);
                dyParam.Add("@pi_contrasenha", Crypto.Encrypt(credenciales.Password));
                dyParam.Add("@pi_id_canal", idCanal);
                dyParam.Add("@po_salida", dbType: DbType.String, direction: ParameterDirection.Output, size: 10);

                var query = "servicios.sp_valida_autenticacion_x_canal";

                SqlMapper.ExecuteScalar<int>(connection, query, param: dyParam,
                    commandType: CommandType.StoredProcedure);

                var respuesta = dyParam.Get<string>("@po_salida");

                ok = respuesta.Equals("S") ? true : false;
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }

            return ok;
        }

        public UsuarioFinansys GetDatosUsuarioFsys(string usuario)
        {
            var connection = new OracleConnection(_connectionStringSeguridad);

            try
            {
                var dyParam = new OracleDynamicParameters();
                dyParam.Add("pi_usuario", OracleDbType.Varchar2, ParameterDirection.Input, usuario);
                dyParam.Add("po_datos", OracleDbType.RefCursor, ParameterDirection.Output);

                var query = "servicios.sp_datosUsuarioFinansys";

                return SqlMapper.QuerySingle<UsuarioFinansys>(connection, query, param: dyParam,
                    commandType: CommandType.StoredProcedure);
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }

        public ResponseAutorizacionConsumoApi ValidarSeguridadToken(string canal, string ipRemota, string url,
            string codigoRol, int cotizable)
        {
            var oracleConexion = new OracleConnection(_connectionStringSeguridad);
            try
            {
                oracleConexion.Open();

                var dyParam = new OracleDynamicParameters();
                dyParam.Add("pi_canal", OracleDbType.Varchar2, ParameterDirection.Input, canal);
                dyParam.Add("pi_ip", OracleDbType.Varchar2, ParameterDirection.Input, ipRemota);
                dyParam.Add("pi_rol", OracleDbType.Varchar2, ParameterDirection.Input, codigoRol);
                dyParam.Add("pi_serv_coti", OracleDbType.Int32, ParameterDirection.Input, cotizable);
                dyParam.Add("pi_recurso", OracleDbType.Varchar2, ParameterDirection.Input, url);
                dyParam.Add("po_respuesta", OracleDbType.RefCursor, ParameterDirection.Output);

                var query = "servicios.sp_valida_seguridad_token";

                var result = SqlMapper.QuerySingle<ResponseAutorizacionConsumoApi>(oracleConexion, query,
                    param: dyParam, commandType: CommandType.StoredProcedure);

                return result;
            }
            finally
            {
                oracleConexion.Close();
                oracleConexion.Dispose();
            }
        }
    }
}
