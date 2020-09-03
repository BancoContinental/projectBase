using Continental.API.Seguridad.Utiles;

namespace Continental.API.Seguridad.Dominio
{
    public class Canal
    {
        public string IdToken { get; set; }

        public string SecretKeyToken { get; set; }

        public int idCanal { get; set; }

        public string CanalNombre { get; set; }

        public int DuracionToken { get; set; }

        public string Origen { get; set; }

        public string TieneRolGenerico { get; set; }

        public int RolGenerico { get; set; }

        public string ipRemota { get; set; }

        public TipoAutenticacion TipoDeAutenticacion { get; set; }
    }
}
