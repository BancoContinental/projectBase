using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Continental.API.Seguridad;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Continental.API.WebApi.Middlewares
{
	// You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
	public class VerifyTokenSenderMiddleWare
	{
		private readonly RequestDelegate _next;

		private readonly ILogger<VerifyTokenSenderMiddleWare> _logger;

		public readonly IOperacionesToken _operacionesToken;

		/// <summary>
		/// Initializes a new instance of the <see cref="VerifyTokenSenderMiddleWare"/> class.
		/// </summary>
		/// <param name="next">next.</param>
		/// <param name="logger">logger.</param>
		/// <param name="operacionesToken">operacionesToken.</param>
		public VerifyTokenSenderMiddleWare(
			RequestDelegate next,
			ILogger<VerifyTokenSenderMiddleWare> logger,
			IOperacionesToken operacionesToken)
		{
			_next = next;
			_logger = logger;
			_operacionesToken = operacionesToken;
		}

		public async Task Invoke(HttpContext httpContext)
		{
			try
			{
				if (httpContext.Request.Path.Value.ToLower().Contains("swagger"))
				{
					await _next(httpContext);
					return;
				}

				bool isAuthenticated = httpContext.User.Identity.IsAuthenticated;

				if (!isAuthenticated)
				{
					_logger.LogWarning("Acceso denegado, no autenticado.");

					httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

					return;
				}

				if (httpContext.User.Identity.AuthenticationType.ToLower().Equals("authenticationtypes.federation"))
				{
					// Validacion de cabecera, existe el token
					IHeaderDictionary headers = httpContext.Request.Headers;

					var tokenStr = _operacionesToken.ExtraerElTokenDeLaCabecera(headers);

					if (string.IsNullOrEmpty(tokenStr))
					{
						_logger.LogWarning("Acceso denegado, no se pudo extraer el Token de la cabecera.");

						httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

						return;
					}

					// Desglozar el token
					var handler = new JwtSecurityTokenHandler();
					var tokenSecurity = handler.ReadToken(tokenStr) as JwtSecurityToken;
					var canal = tokenSecurity.Claims.First(claim => claim.Type == "canal").Value;
					var usuario = tokenSecurity.Claims.First(claim1 => claim1.Type == "sub").Value;

					bool esServicioCotizable = tokenSecurity.Claims.First(claim => claim.Type == "serv").Value.Equals("S");

					// Cuando el token tenga como fin acceder a un servico que sea cotizable, se utiliza el código de cliente asociado al token para controlar el ROL.
					var rol = esServicioCotizable
						? tokenSecurity.Claims.First(claim => claim.Type == "servClient").Value
						: tokenSecurity.Claims.First(claim => claim.Type == "rol").Value;

					if (!_operacionesToken.EsUnCanalHabilitado(canal))
					{
						_logger.LogWarning("Acceso denegado Canal inhabilitado");

						httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

						return;
					}

					// Validación de la IP
					var ipRemota = httpContext.Connection.RemoteIpAddress.ToString();

					if (!_operacionesToken.EsUnaIpRemotaHabilitada(canal, ipRemota))
					{
						_logger.LogWarning("Acceso denegado IP inhabilitada");

						httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

						return;
					}

					// Validación de acceso al recurso
					var urlDelRecurso = string.Concat(httpContext.Request.PathBase.ToUriComponent(), httpContext.Request.Path.ToUriComponent());

					if (!_operacionesToken.EsUnRecursoHabilitadoPorRol(urlDelRecurso, rol, esServicioCotizable))
					{
						_logger.LogWarning("Acceso denegado, Url no accesible para este token (url {0}, rol/depart {1})", urlDelRecurso, rol);

						httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

						return;
					}
				}

				_logger.LogInformation("Controles personalizados exitosos");

				await _next(httpContext);
			}
			catch (System.Exception ex)
			{
				_logger.LogError(ex, "Ocurrio un error al validar el token");

				httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
			}
		}
	}

	// Extension method used to add the middleware to the HTTP request pipeline.
	public static class VerifyTokenSenderMiddleWareExtensions
	{
		public static IApplicationBuilder UseVerifyTokenSenderMiddleWare(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<VerifyTokenSenderMiddleWare>();
		}
	}
}
