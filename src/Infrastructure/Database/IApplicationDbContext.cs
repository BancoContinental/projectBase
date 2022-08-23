using Continental.API.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Continental.API.Infrastructure.Database;

public interface IApplicationDbContext
{
    DbSet<Feriado> Feriados { get; set; }
}