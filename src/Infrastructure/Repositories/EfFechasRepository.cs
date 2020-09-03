using System;
using System.Threading.Tasks;
using Continental.API.Core.Entities;
using Continental.API.Core.Interfaces;
using Continental.API.Infrastructure.Data;
using Continental.API.Infrastructure.DatabaseHelpers;
using Continental.API.Infrastructure.Settings;
using Continental.API.Infrastructure.Settings.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Continental.API.Infrastructure.Repositories
{
    public class EfFechasRepository : IFechasRepository
    {
        private readonly OracleOracleDbContext _context;
        private readonly ConexionBD conexionesBD;

        public EfFechasRepository(OracleOracleDbContext context, IOptions<Configuraciones>  configuraciones)
        {
            _context     = context;
            conexionesBD = new ConexionBD(configuraciones.Value.SeteosBD);
        }

        public async Task<bool> EsDiaHabil(DateTime fecha, CredencialesFinansys credenciales = null)
        {
            if (!(credenciales is null))
            {
                return await EsDiaHabil(fecha, credenciales.UsuarioOracle, credenciales.Password);
            }

            var resultado = await _context.Feriados.CountAsync(e => e.FechaFeriado == fecha.Date);

            return !(resultado > 0);
        }

        private async Task<bool> EsDiaHabil(DateTime fecha, string usuario, string password)
        {
            var context = new TresLetrasOracleDbContext(
                conexionesBD.GetCadenaDeConexion(usuario, password, TiposDataSource.DATOSITA));

            var resultado = await context.Feriados.CountAsync(e => e.FechaFeriado == fecha.Date);

            return !(resultado > 0);
        }
    }
}
