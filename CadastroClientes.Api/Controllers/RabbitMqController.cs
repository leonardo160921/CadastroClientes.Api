using Microsoft.AspNetCore.Mvc;
using CadastroClientes.Api.Messaging;

namespace CadastroClientes.Api.Controllers;

[ApiController]
[Route("api/rabbitmq")]
public class RabbitMqController : ControllerBase
{
    private readonly IRabbitMqService _rabbitMqService;

    public RabbitMqController(IRabbitMqService rabbitMqService)
    {
        _rabbitMqService = rabbitMqService;
    }

    [HttpPost("publicar")]
    public async Task<IActionResult> Publicar([FromBody] string mensagem)
    {
        await _rabbitMqService.PublicarAsync(mensagem);

        return Ok(new
        {
            mensagem = "Mensagem publicada com sucesso."
        });
    }
}