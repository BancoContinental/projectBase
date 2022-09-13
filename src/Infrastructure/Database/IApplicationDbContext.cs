using System.Threading;
using System.Threading.Tasks;
using Continental.API.Core.Contracts.Entities;
using Microsoft.EntityFrameworkCore;

namespace Continental.API.Infrastructure.Database;

public interface IApplicationDbContext
{
    public DbSet<CuentaCorriente> CuentasCorrientes { get; set; }
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());

}