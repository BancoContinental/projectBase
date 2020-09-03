namespace Continental.API.Seguridad.Dominio
{
    public class Credenciales
    {
        public Credenciales(string usuario, string password)
        {
            this.Usuario = usuario;
            this.Password = password;
        }
        public string Usuario { get; set; }
        public string Password { get; set; }
    }
}
