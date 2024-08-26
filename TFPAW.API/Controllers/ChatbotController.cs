using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ChatbotController : ControllerBase
{
    private readonly ChatService _chatService;

    public ChatbotController(ChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage([FromBody] ChatRequest request)
    {
        if (request?.Message == null)
        {
            return BadRequest("El campo 'message' es requerido.");
        }

        var response = await _chatService.SendMessageAsync(request.Message);

        if (string.IsNullOrEmpty(response))
        {
            return NoContent(); // Devuelve 204 si no hay contenido
        }

        return Ok(response); // Devuelve 200 con la respuesta
    }
}

public class ChatRequest
{
    public string Message { get; set; }
}
