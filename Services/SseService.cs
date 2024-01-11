namespace SseDemo.Services;

public class SseService
{
    private readonly ILogger<SseService> _logger;

    public SseService(ILogger<SseService> logger)
    {
        _logger = logger;
    }

    public event EventHandler<string>? MessageEvent;

    public void Broadcast(string message)
    {
        _logger.LogInformation("Broadcasting event to all event listeners: {Message}", message);
        MessageEvent?.Invoke(this, message);
    }
}