using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace CadastroClientes.Api.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/clientes")]
    [ApiExplorerSettings(GroupName = "v2")]
    public class ClientesControllerV2 : ControllerBase
    {
        [HttpGet]
        public IActionResult ObterTodos()
        {
            return Ok(new
            {
                Mensagem = "API versão 2"
            });
        }
    }
}
