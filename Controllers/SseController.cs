using Microsoft.AspNetCore.Mvc;
using SseDemo.Services;

namespace SseDemo.Controllers;

[ApiController]
[Route("[controller]")]
public class SseController : ControllerBase
{
    private readonly ILogger<SseController> _logger;
    private readonly SseService _sseService;

    public SseController(ILogger<SseController> logger, SseService sseService)
    {
        _logger = logger;
        _sseService = sseService;
    }

    [HttpGet]
    public async Task Get(CancellationToken cancellationToken)
    {
        Response.Headers.Add("Content-Type", "text/event-stream");
        Response.Headers.Add("Cache-Control", "no-cache");
        Response.Headers.Add("Connection", "keep-alive");

        _sseService.MessageEvent += OnMessage;

        await KeepAliveUntilCanceled(cancellationToken);
    }

    private async void OnMessage(object? sender, string message)
    {
        await Response.WriteAsync($"data:{message}\n\n");
        await Response.Body.FlushAsync();
    }

    private async Task KeepAliveUntilCanceled(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1000, cancellationToken);
            }
        }
        catch (TaskCanceledException)
        {
            _logger.LogDebug("Task has been canceled. User likely disconnected.");
        }
        finally
        {
            _sseService.MessageEvent -= OnMessage;
        }
    }
}
