using System.Collections.Generic;
using System.Data;
using Dapper;
using Oracle.ManagedDataAccess.Client;

namespace Continental.API.Seguridad.Utiles
{
    class OracleDynamicParameters : SqlMapper.IDynamicParameters
    {
        private readonly DynamicParameters dynamicParameters = new DynamicParameters();
        private readonly List<OracleParameter> oracleParameters = new List<OracleParameter>();

        /// <summary>
        /// Metodo de Conexion.
        /// </summary>
        /// <param name="name">name.</param>
        /// <param name="oracleDbType">oracle.</param>
        /// <param name="direction">direction.</param>
        /// <param name="value">value.</param>
        /// <param name="size">size.</param>
        public void Add(string name, OracleDbType oracleDbType, ParameterDirection direction, object value = null, int? size = null)
        {
            OracleParameter oracleParameter;
            if (size.HasValue)
            {
                oracleParameter = new OracleParameter(name, oracleDbType, size.Value, value, direction);
            }
            else
            {
                oracleParameter = new OracleParameter(name, oracleDbType, value, direction);
            }

            this.oracleParameters.Add(oracleParameter);
        }

        /// <summary>
        /// Metodo de Conexion.
        /// </summary>
        /// <param name="name">name.</param>
        /// <param name="oracleDbType">oracle.</param>
        /// <param name="direction">direction.</param>
        public void Add(string name, OracleDbType oracleDbType, ParameterDirection direction)
        {
            var oracleParameter = new OracleParameter(name, oracleDbType, direction);
            this.oracleParameters.Add(oracleParameter);
        }

        /// <summary>
        /// Metodo de add Parameters.
        /// </summary>
        /// <param name="command">name.</param>
        /// <param name="identity">identity.</param>
        public void AddParameters(IDbCommand command, SqlMapper.Identity identity)
        {
            ((SqlMapper.IDynamicParameters)this.dynamicParameters).AddParameters(command, identity);

            var oracleCommand = command as OracleCommand;

            if (oracleCommand != null)
            {
                oracleCommand.Parameters.AddRange(this.oracleParameters.ToArray());
            }
        }
    }
}
