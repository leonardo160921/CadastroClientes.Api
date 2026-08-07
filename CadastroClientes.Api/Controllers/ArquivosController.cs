using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace APIGerenciadorTarefas.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArquivosController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;

    public ArquivosController(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    [HttpGet("download/{nomeArquivo}")]
    public IActionResult Download(string nomeArquivo)
    {
        var caminho = Path.Combine(
            _environment.ContentRootPath,
            "Uploads",
            nomeArquivo);

        if (!System.IO.File.Exists(caminho))
            return NotFound("Arquivo não encontrado.");

        var provider = new FileExtensionContentTypeProvider();

        if (!provider.TryGetContentType(caminho, out string? contentType))
        {
            contentType = "application/octet-stream";
        }

        return PhysicalFile(
            caminho,
            contentType,
            nomeArquivo);
    }
}