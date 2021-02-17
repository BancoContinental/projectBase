namespace Continental.API.Infrastructure.DatabaseHelpers
{
    internal static class ConexionBD
    {
        public static string ArmarCadenaDeConexion(string template, string usuario, string password)
            => string.Format(template, usuario, password);
    }
}
