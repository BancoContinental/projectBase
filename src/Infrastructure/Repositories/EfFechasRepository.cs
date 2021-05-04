using System;
using System.Threading.Tasks;
using Continental.API.Core.Interfaces;
using Continental.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Continental.API.Infrastructure.Repositories
{
    public class EfFechasRepository : IFechasRepository
    {
        private readonly OracleOracleDbContext _context;

        public EfFechasRepository(OracleOracleDbContext context)
        {
            _context = context;
        }

        public async Task<bool> EsDiaHabil(DateTime fecha)
        {
            var resultado = await _context.Feriados.CountAsync(e => e.FechaFeriado == fecha.Date);

            return !(resultado > 0);
        }
    }
}
