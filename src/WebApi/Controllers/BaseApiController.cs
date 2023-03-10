using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Continental.API.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : Controller
{
    private readonly IMapper _mapper = default;
    protected IMapper Mapper => _mapper ?? HttpContext.RequestServices.GetService<IMapper>();
}
