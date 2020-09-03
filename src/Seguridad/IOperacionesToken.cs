using System.Collections.Generic;
using Continental.API.Seguridad.Dominio;
using Microsoft.AspNetCore.Http;

namespace Continental.API.Seguridad
{
	public interface IOperacionesToken
	{
		/// <summary>
		/// Método que realiza la validación del canal que realiza la petición, debe estar registrado en la Base de Datos
		/// </summary>
		/// <param name="canal">Canal registrado en la BD para poder acceder a las APIs.</param>
		/// <returns>True or False de acuerdo a si está o no registrado en la BD</returns>
		bool EsUnCanalHabilitado(string canal);

		/// <summary>
		/// Método que realiza la validación de la IP remota que realiza la petición, la misma debe estar activa y asociada a un canal habilitado.
		/// </summary>
		/// <param name="ip">Dirección IP remota registrada para poder acceder a las APIs.</param>
		/// <returns>True or False de acuerdo a si está o no registrado en la BD.</returns>
		bool EsUnaIpRemotaHabilitada(string canal, string ip);

		/// <summary>
		/// Método que realiza la validación de acceso a un recurso de API mediante el Roles/Departamento de un usuario
		/// </summary>
		/// <param name="url">Url de acceso al recurso.</param>
		/// <param name="rol">Rol del usuario que realiza la petición.</param>
		/// <returns>True or False de acuerdo a </returns>
		bool EsUnRecursoHabilitadoPorRol(string url, string rolAverificar, bool esExterno);

		/// <summary>
		/// Metodo que realiza la validación de las credenciales de Finansys, solo intentamos autenticarnos a la BD.
		/// </summary>
		/// <param name="usuario">UsuarioFinansys de Finansys.</param>
		/// <param name="contrasenha">Contraseña de Finansys.</param>
		/// <returns>True or False si se logra autenticar a la Base de Datos.</returns>
		bool VerificaAutenticacionParaFinansys(string usuario, string contrasenha);

		/// <summary>
		/// Metodo para obtener el Rol asociado al usuario solicitado.
		/// </summary>
		/// <param name="usuario">UsuarioFinansys que realiza la petición.</param>
		/// <returns>El rolAverificar del usuario.</returns>
		string GetRolPorUsuario(string usuario);

		/// <summary>
		/// Método para obtener la clave del Token
		/// </summary>
		/// <param name="idToken">Identificador del Token.</param>
		/// <returns>Secret key utilizado para la firma del Token.</returns>
		string GetKeyDelToken(string idToken);

		/// <summary>
		/// Método para extraer el Token de la cabecera de la petición.
		/// </summary>
		/// <param name="headers">Coleccion de cabeceras de la petición.</param>
		/// <returns>El Token.</returns>
		string ExtraerElTokenDeLaCabecera(IHeaderDictionary headers);

		/// <summary>
		/// Extrae del token el claim solicitado.
		/// </summary>
		/// <param name="token">el JWT.</param>
		/// <param name="claimType">Tipo de claim.</param>
		/// <returns>El valor del tipo de claim solicitado.</returns>
		string ExtraerClaimDelToken(string token, string claimType);

		/// <summary>
		/// Obtiene los datos de un Canal a partir del nombre de un canal.
		/// </summary>
		/// <param name="nombreCanal">nombre del canal.</param>
		/// <returns>objeto Canal.</returns>
		Canal ObtenerDatosDelCanalPorNombreCanal(string nombreCanal);

		/// <summary>
		/// Obtiene el código de cliente externo a partir del identificador de canal y la direccion IP.
		/// </summary>
		/// <param name="idCanal">Identificador del canal.</param>
		/// <param name="ipRemota">Direccion IP.</param>
		/// <returns></returns>
		string GetCodigoClienteExternoPorIp(int idCanal, string ipRemota);

		/// <summary>
		/// Obtiene todos los canales asociados a una determinada IP.
		/// </summary>
		/// <param name="ipRemota">direccion IP remota.</param>
		/// <returns>Lista de canales.</returns>
		List<Canal> GetCanalesAsociadosPorIp(string ipRemota);

		/// <summary>
		/// Metodo que valida la autenticación de banca empresas.
		/// </summary>
		/// <param name="credenciales">Objeto que contiene usuario y contraseña.</param>
		/// <param name="codigoCliente">Codigo de cliente de la empresa.</param>
		/// <returns>true or false segun el resultado de la autenticacion.</returns>
		bool AutenticaBancaEmpresas(Credenciales credenciales, string codigoCliente);

		/// <summary>
		/// Método que valida la autenticacion por Canal
		/// </summary>
		/// <param name="credenciales">Objeto que contiene usuario y contraseña.</param>
		/// <param name="idCanal">Identificador del canal.</param>
		/// <returns>true or false segun el resultado de la autenticacion.</returns>
		bool AutenticacionPorCanal(Credenciales credenciales, int idCanal);

		/// <summary>
		/// Método que obtiene los datos de finansys a partir del usuario de domminio.
		/// </summary>
		/// <param name="usuario">UsuarioFinansys de dominio</param>
		/// <returns></returns>
		UsuarioFinansys GetDatosUsuarioFsys(string usuario);

		/// <summary>
		/// Método que valida la autorizacion del token por canal, ip y url
		/// </summary>
		/// <param name="canal">canal que se obtiene del token</param>
		/// <param name="ip">Ip que se obtiene del token</param>
		/// <param name="url">url que se obtiene del token</param>
		/// <param name="codigoRol">codigo del rol o codigo de cliente que se obtiene del token</param>
		/// <param name="cotizable">si el servicio es cotizable, busca por clientes, sino por rol</param>
		/// <returns></returns>
		ResponseAutorizacionConsumoApi GetAutorizacionConsumoApi(string canal, string ip, string url, string codigoRol, int cotizable);
	}
}
