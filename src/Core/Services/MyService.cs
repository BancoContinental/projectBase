using Continental.API.Core.Interfaces;

namespace Continental.API.Core.Services;

public class MyService : IMyService
{
    private readonly IMyRepository _myRepository;

    public MyService(IMyRepository myRepository)
    {
        _myRepository = myRepository;
    }
}
