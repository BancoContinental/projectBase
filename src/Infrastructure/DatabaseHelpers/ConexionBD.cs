using System.Linq;
using Continental.API.Infrastructure.Settings.DataBase;

namespace Continental.API.Infrastructure.DatabaseHelpers
{
    internal class ConexionBD
    {
        private SeteosBD _seteos;

        public ConexionBD(SeteosBD seteos)
        {
            _seteos = seteos;
        }

        public string GetCadenaDeConexion(TiposCredenciales credenciales, TiposDataSource source)
        {
            var credencial = _seteos.Credenciales.FirstOrDefault(t => t.Key.ToUpper().Equals(credenciales.ToString().ToUpper()));

            var dataSource = _seteos.Datasources.FirstOrDefault(t => t.Key.ToUpper().Equals(source.ToString().ToUpper()));

            return armaCadenaDeConexion(_seteos.EsProduccion, credencial, dataSource);
        }

        public string GetCadenaDeConexion(string usuario, string password, TiposDataSource source)
        {
            var dataSource = _seteos.Datasources.FirstOrDefault(t => t.Key.ToUpper().Equals(source.ToString().ToUpper()));

            return armaCadenaDeConexion(_seteos.EsProduccion, usuario, password, dataSource);
        }

        string armaCadenaDeConexion(bool esProduccion, Credencial credencial, DataSource dataSource)
            => armaCadenaDeConexion(esProduccion, credencial.Usuario,
                esProduccion ? credencial.PasswordProduccion : credencial.PasswordDesarrollo, dataSource);

        string armaCadenaDeConexion(bool esProduccion, string usuario, string contrasenha, DataSource dataSource)
        {
            var source = esProduccion ? dataSource.SourceProduccion : dataSource.SourceDesarrollo;

            return $"User Id={usuario};Password={contrasenha};Data Source={source};Connection Timeout=120;Pooling=false";
        }
    }
}
