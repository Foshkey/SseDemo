namespace SseDemo.Services;

public class SseService
{
    private readonly List<HttpContext> _clients = new();

    public async Task EstablishSseConnection(HttpContext context)
    {
        context.Response.Headers.Add("Content-Type", "text/event-stream");
        context.Response.Headers.Add("Cache-Control", "no-cache");
        context.Response.Headers.Add("Connection", "keep-alive");

        // Notify when the client connects
        _clients.Add(context);

        // Handle client disconnection
        context.RequestAborted.Register(() => _clients.Remove(context));

        // Keep looping until a cancellation was requested
        while (!context.RequestAborted.IsCancellationRequested)
        {
            await Task.Delay(1000);
        }
    }

    public Task SendToAllClients(string message)
        => Task.WhenAll(_clients.Select(async client =>
            {
                var formattedMessage = $"data: {message}\n\n";
                await client.Response.WriteAsync(formattedMessage);
                await client.Response.Body.FlushAsync();
            }));
}