using System.Collections.Generic;
using System.Data;
using Dapper;
using Oracle.ManagedDataAccess.Client;

namespace Continental.API.Infrastructure.DatabaseHelpers;

internal class OracleDynamicParameters : SqlMapper.IDynamicParameters
{
    private readonly DynamicParameters _dynamicParameters = new DynamicParameters();
    private readonly List<OracleParameter> _oracleParameters = new List<OracleParameter>();

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

        this._oracleParameters.Add(oracleParameter);
    }

    public void Add(string name, OracleDbType oracleDbType, ParameterDirection direction)
    {
        var oracleParameter = new OracleParameter(name, oracleDbType, direction);
        this._oracleParameters.Add(oracleParameter);
    }

    public void AddParameters(IDbCommand command, SqlMapper.Identity identity)
    {
        ((SqlMapper.IDynamicParameters)this._dynamicParameters).AddParameters(command, identity);

        var oracleCommand = command as OracleCommand;

        if (oracleCommand != null)
        {
            oracleCommand.Parameters.AddRange(this._oracleParameters.ToArray());
        }
    }
}