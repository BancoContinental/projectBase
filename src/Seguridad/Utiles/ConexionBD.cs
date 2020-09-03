using System.Linq;
using Continental.API.Seguridad.Settings.DataBase;

namespace Continental.API.Seguridad.Utiles
{
	internal class ConexionBD
	{
        private SeteosBD seteosBD;

        public ConexionBD(SeteosBD seteosBD)
        {
            this.seteosBD = seteosBD;
        }

        public string GetCadenaDeConexion(TiposCredenciales credenciales, TiposDataSource source)
        {
            var credencial = this.seteosBD.Credenciales.Where(t => t.Key.ToUpper().Equals(credenciales.ToString().ToUpper())).FirstOrDefault();

            var dataSource = this.seteosBD.Datasources.Where(t => t.Key.ToUpper().Equals(source.ToString().ToUpper())).FirstOrDefault();

            return this.armaCadenaDeConexion(this.seteosBD.EsProduccion, credencial, dataSource);
        }

        public string GetCadenaDeConexion(string usuario, string password, TiposDataSource source)
        {
            var dataSource = this.seteosBD.Datasources.Where(t => t.Key.ToUpper().Equals(source.ToString().ToUpper())).FirstOrDefault();

            return this.armaCadenaDeConexion(this.seteosBD.EsProduccion, usuario, password, dataSource);
        }

        string armaCadenaDeConexion(bool esProduccion, Credencial credencial, DataSource dataSource)
        {
            return string.Format("User Id={0};Password={1};Data Source={2};Connection Timeout=120;Pooling=false",
                credencial.Usuario,
                esProduccion ? credencial.PasswordProduccion : credencial.PasswordDesarrollo,
                esProduccion ? dataSource.SourceProduccion : dataSource.SourceDesarrollo);
        }

        string armaCadenaDeConexion(bool esProduccion, string usuario, string contrasenha, DataSource dataSource)
        {
            return string.Format("User Id={0};Password={1};Data Source={2};Connection Timeout=120;Pooling=false",
                usuario,
                contrasenha,
                esProduccion ? dataSource.SourceProduccion : dataSource.SourceDesarrollo);
        }
    }
}
