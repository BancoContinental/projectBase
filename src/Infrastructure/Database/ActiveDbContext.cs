﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Continental.API.Core.Contracts.Data;
using Microsoft.EntityFrameworkCore;

namespace Continental.API.Infrastructure.Database;

/// <summary>
/// DbContext para lecturas de la base. Debe usar conexion a ACTIVE
/// </summary>
public sealed class ActiveDbContext : DbContext, IApplicationDbContext
{
    private static readonly ApplicationException ReadOnlyDatabaseException = new("Base de datos de solo lectura!");

    public DbSet<CuentaCorrienteDto> CuentasCorrientes { get; set; }

    public override int SaveChanges() => throw ReadOnlyDatabaseException;

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken()) => throw ReadOnlyDatabaseException;
}
