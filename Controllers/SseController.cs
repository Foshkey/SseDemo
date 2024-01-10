using Microsoft.AspNetCore.Mvc;
using SseDemo.Services;

namespace SseDemo.Controllers;

[ApiController]
[Route("[controller]")]
public class SseController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromServices] SseService sseService)
    {
        await sseService.EstablishSseConnection(HttpContext);
        return new EmptyResult();
    }
}
