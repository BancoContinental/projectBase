using System;
using System.Threading.Tasks;
using Continental.API.Core.Entities;
using Continental.API.Core.Interfaces;
using Continental.API.Infrastructure.Data;
using Continental.API.Infrastructure.DatabaseHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Continental.API.Infrastructure.Repositories
{
    public class EfFechasRepository : IFechasRepository
    {
        private readonly OracleOracleDbContext _db;
        private readonly IConfiguration _configuration;

        public EfFechasRepository(OracleOracleDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        public async Task<bool> EsDiaHabil(DateTime fecha, CredencialesFinansys credenciales = null)
        {
            if (!(credenciales is null))
            {
                return await EsDiaHabil(fecha, credenciales.UsuarioOracle, credenciales.Password);
            }

            var resultado = await _db.Feriados.CountAsync(e => e.FechaFeriado == fecha.Date);

            return !(resultado > 0);
        }

        private async Task<bool> EsDiaHabil(DateTime fecha, string usuario, string password)
        {
            var context = new TresLetrasOracleDbContext(ConexionBD.ArmarCadenaDeConexion(
                _configuration.GetConnectionString("FinansysWeb"),
                usuario,
                password));

            var resultado = await context.Feriados.CountAsync(e => e.FechaFeriado == fecha.Date);

            return !(resultado > 0);
        }
    }
}
