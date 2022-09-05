using Continental.API.Core.Interfaces;
using Continental.API.Infrastructure.Database;

namespace Continental.API.Infrastructure.Repositories;

public class MyRepository : IMyRepository
{
    private readonly IDbUnitOfWork _db;

    public MyRepository(IDbUnitOfWork db)
    {
        _db = db;
    }
}