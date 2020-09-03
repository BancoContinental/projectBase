using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text.RegularExpressions;
using Continental.API.Seguridad.Dominio;
using Continental.API.Seguridad.Repositorio;
using Continental.API.Seguridad.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Continental.API.Seguridad
{
	public class OperacionesToken : IOperacionesToken
	{
		private readonly Configuraciones configuraciones;

		private readonly TokenRepositorio repositorio;

		/// <summary>
		/// Constructor de la clase
		/// </summary>
		public OperacionesToken (IOptions<Configuraciones> options)
		{
			this.configuraciones = options.Value;

			this.repositorio = new TokenRepositorio (this.configuraciones);
		}

		/// <summary>
		/// Método que realiza la validación del canal que realiza la petición, debe estar registrado en la Base de Datos
		/// </summary>
		/// <param name="canal">Canal registrado en la BD para poder acceder a las APIs.</param>
		/// <returns>True or False de acuerdo a si está o no registrado en la BD</returns>
		public bool EsUnCanalHabilitado ( string canal )
		{
			try
			{
				string idToken        = this.repositorio.GetIdToken ();
				string [ ] losCanales = this.repositorio.GetCanalesPorToken ( idToken );

				return ElValorSeEncuentraDentroDelArray ( losCanales , canal );
			}
			catch ( Exception ex )
			{
				throw ex;
			}
		}

		/// <summary>
		/// Método que realiza la validación de la IP remota que realiza la petición, la misma debe estar activa y asociada a un canal habilitado.
		/// </summary>
		/// <param name="ip">Dirección IP remota registrada para poder acceder a las APIs.</param>
		/// <returns>True or False de acuerdo a si está o no registrado en la BD.</returns>
		public bool EsUnaIpRemotaHabilitada ( string canal , string ip )
		{
			try
			{
				string [ ] lasDireccionesIp = this.repositorio.GetDireccionesIpPorCanal ( canal );

				return ElValorSeEncuentraDentroDelArray ( lasDireccionesIp , ip );
			}
			catch ( Exception ex )
			{
				throw ex;
			}
		}

		/// <summary>
		/// Método que realiza la validación de acceso a un recurso de API mediante el Roles/Departamento de un usuario
		/// </summary>
		/// <param name="url">Url de acceso al recurso.</param>
		/// <param name="rol">Rol del usuario que realiza la petición.</param>
		/// <param name="esServicioCotizable">Determina si es un servicio cotizable.</param>
		/// <returns>True or False de acuerdo a </returns>
		public bool EsUnRecursoHabilitadoPorRol ( string url , string rol, bool esServicioCotizable )
		{
			try
			{
				var listaDeRecursos = this.repositorio.GetRecursosHabilitados ();

				var recurso = this.GetDatosDelRecurso ( listaDeRecursos , url );

				if ( recurso == null )
				{
					return false;
				}

				string[] rolesDisponibles = esServicioCotizable
					? recurso.Clientes.Split(',')
					: recurso.Roles.Split(',');

				return ElValorSeEncuentraDentroDelArray ( rolesDisponibles , rol );
			}
			catch ( Exception ex )
			{
				throw ex;
			}
		}

		/// <summary>
		/// Metodo que realiza la validación de las credenciales de Finansys, solo intentamos autenticarnos a la BD.
		/// </summary>
		/// <param name="usuario">UsuarioFinansys de Finansys.</param>
		/// <param name="contrasenha">Contraseña de Finansys.</param>
		/// <returns>True or False si se logra autenticar a la Base de Datos.</returns>
		public bool VerificaAutenticacionParaFinansys ( string usuario , string contrasenha )
		{
			try
			{
				return this.repositorio.ValidateAuthenticationFsys ( usuario , contrasenha );
			}
			catch ( Exception ex )
			{
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para obtener el Rol asociado al usuario solicitado.
		/// </summary>
		/// <param name="usuario">UsuarioFinansys que realiza la petición.</param>
		/// <returns>El rol del usuario.</returns>
		public string GetRolPorUsuario ( string usuario )
		{
			try
			{
				return this.repositorio.GetRolPorUsuario ( usuario );
			}
			catch ( Exception ex )
			{
				throw ex;
			}
		}

		/// <summary>
		/// Método para obtener la clave del Token
		/// </summary>
		/// <param name="idToken">Identificador del Token.</param>
		/// <returns>Secret key utilizado para la firma del Token.</returns>
		public string GetKeyDelToken ( string idToken )
		{
			try
			{
				return this.repositorio.GetSecretKeyPorIdToken ( idToken );
			}
			catch ( Exception ex )
			{
				throw ex;
			}
		}

		/// <summary>
		/// Método para extraer el Token de la cabecera de la petición.
		/// </summary>
		/// <param name="headers">Coleccion de cabeceras de la petición.</param>
		/// <returns>El Token.</returns>
		public string ExtraerElTokenDeLaCabecera ( IHeaderDictionary headers )
		{
			string tokenString = string.Empty;

			try
			{
				if (headers.ContainsKey("Authorization"))
				{
					var headerAuth = headers["Authorization"].FirstOrDefault().Trim();
					bool isBearer  = headerAuth.ToLower().Contains("bearer");

					if (isBearer)
					{
						tokenString = Regex.Replace(headerAuth, "bearer ", string.Empty, RegexOptions.IgnoreCase);
					}
				}
			}
			catch ( Exception ex )
			{
				throw ex;
			}

			return tokenString;
		}

		/// <summary>
		/// Extrae del token el claim solicitado.
		/// </summary>
		/// <param name="token">el JWT.</param>
		/// <param name="claimType">Tipo de claim.</param>
		/// <returns>El valor del tipo de claim solicitado.</returns>
		public string ExtraerClaimDelToken(string token, string claimType)
		{
			var handler = new JwtSecurityTokenHandler();
			var tokenSecurity = handler.ReadToken(token) as JwtSecurityToken;
			return tokenSecurity.Claims.First(claim => claim.Type == claimType).Value;
		}

		/// <summary>
		/// Obtiene los datos de un Canal a partir del nombre de un canal.
		/// </summary>
		/// <param name="nombreCanal">nombre del canal.</param>
		/// <returns>objeto Canal.</returns>
		public Canal ObtenerDatosDelCanalPorNombreCanal(string nombreCanal)
		{
			try
			{
				return this.repositorio.GetDatosCanalPorId(nombreCanal);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// Obtiene el código de cliente externo a partir del identificador de canal y la direccion IP.
		/// </summary>
		/// <param name="idCanal">Identificador del canal.</param>
		/// <param name="ipRemota">Direccion IP.</param>
		/// <returns></returns>
		public string GetCodigoClienteExternoPorIp(int idCanal, string ipRemota)
		{
			try
			{
				return this.repositorio.GetClienteExternoPorIp(idCanal, ipRemota);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// Obtiene todos los canales asociados a una determinada IP.
		/// </summary>
		/// <param name="ipRemota">direccion IP remota.</param>
		/// <returns>Lista de canales.</returns>
		public List<Canal> GetCanalesAsociadosPorIp(string ipRemota)
		{
			try
			{
				return this.repositorio.GetCanalesPorIp(ipRemota);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// Método que obtiene los datos de finansys a partir del usuario de domminio.
		/// </summary>
		/// <param name="usuario">UsuarioFinansys de dominio</param>
		/// <returns></returns>
		public UsuarioFinansys GetDatosUsuarioFsys(string usuario)
		{
			try
			{
				return this.repositorio.GetDatosUsuarioFsys(usuario);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public bool AutenticaBancaEmpresas(Credenciales credenciales, string codigoCliente)
		{
			try
			{
				return this.repositorio.AutenticacionBancaCorporativaCorrecta(credenciales, codigoCliente);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public bool AutenticacionPorCanal(Credenciales credenciales, int idCanal)
		{
			try
			{
				return this.repositorio.EsAutenticacionPorCanalCorrecta(credenciales, idCanal);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		bool ElValorSeEncuentraDentroDelArray ( string [ ] cadena , string valorBuscado )
		{
			try
			{
				foreach ( var item in cadena )
				{
					if ( item.ToLower ().Equals ( valorBuscado.ToLower () ) || item.Equals ( "*" ) )
					{
						return true;
					}
				}

				return false;
			}
			catch ( Exception ex )
			{
				throw ex;
			}
		}

		RecursoRol GetDatosDelRecurso ( List<RecursoRol> laLista , string valor )
		{
			try
			{
				var recurso = laLista
					.Where ( t => t.Recurso.ToLower () == valor.ToLower () )
					.FirstOrDefault ();

				if ( recurso == null )
				{
					return null;
				}

				return recurso;
			}
			catch ( Exception ex )
			{
				throw ex;
			}
		}

		/// <summary>
		/// Método que valida la autorizacion del token por canal, ip y url
		/// </summary>
		/// <param name="canal">canal que se obtiene del token</param>
		/// <param name="ip">Ip que se obtiene del token</param>
		/// <param name="url">url que se obtiene del token</param>
		/// <param name="codigoRol">codigo del rol o codigo de cliente que se obtiene del token</param>
		/// <param name="cotizable">si el servicio es cotizable, busca por clientes, sino por rol</param>
		/// <returns></returns>
		public ResponseAutorizacionConsumoApi GetAutorizacionConsumoApi(string canal, string ip, string url, string codigoRol, int cotizable)
		{
			try
			{
				var resultadoValidacion = this.repositorio.ValidarSeguridadToken(canal, ip, url, codigoRol, cotizable);
				return resultadoValidacion;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

	}
}
