using System.Collections.Generic;

namespace Continental.API.Seguridad.Settings.DataBase
{
    public class SeteosBD
    {
        public bool EsProduccion { get; set; }

        public List<Credencial> Credenciales { get; set; }

        public List<DataSource> Datasources { get; set; }
    }
}
