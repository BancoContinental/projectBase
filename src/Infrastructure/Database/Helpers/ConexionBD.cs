namespace Continental.API.Infrastructure.Database.Helpers;

internal static class ConexionBD
{
    public static string ArmarCadenaDeConexion(string template, string usuario, string password)
        => string.Format(template, usuario, password);
}