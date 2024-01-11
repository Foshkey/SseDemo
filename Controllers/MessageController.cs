using Microsoft.AspNetCore.Mvc;
using SseDemo.Services;

namespace SseDemo.Controllers;

[ApiController]
[Route("[controller]")]
public class MessageController : ControllerBase
{
    [HttpPost]
    public IActionResult Post([FromBody] MessageModel model, [FromServices] SseService sseService)
    {
        // Handle the incoming message
        var message = model?.Message;
        if (string.IsNullOrEmpty(message))
        {
            return BadRequest("Invalid message");
        }

        // Send the message to all connected clients
        sseService.Broadcast(message);

        return Ok("Message sent successfully");
    }
}

public class MessageModel
{
    public string? Message { get; set; }
}
