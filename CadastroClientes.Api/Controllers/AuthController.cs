using CadastroClientes.Api.DTOs;
using CadastroClientes.Api.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CadastroClientes.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtOptions _jwt;

    public AuthController(IOptions<JwtOptions> options)
    {
        _jwt = options.Value;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDTO dto)
    {
        // Validação simples apenas para aprendizado
        if (dto.Usuario != "admin" || dto.Senha != "123456")
        {
            return Unauthorized(new
            {
                mensagem = "Usuário ou senha inválidos."
            });
        }

        // Claims do usuário
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, dto.Usuario),
            new Claim(ClaimTypes.Role, "Administrador")
        };

        // Chave secreta
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));

        // Credenciais de assinatura
        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        // Criação do Token
        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_jwt.ExpireMinutes),
            signingCredentials: credentials);

        // Converte o Token para string
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new
        {
            token = tokenString
        });
    }
    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile arquivo)
    {
        if (arquivo == null || arquivo.Length == 0)
            return BadRequest("Arquivo inválido.");

        var caminho = Path.Combine(
            Directory.GetCurrentDirectory(),
            "Uploads",
            arquivo.FileName);

        using (var stream = new FileStream(caminho, FileMode.Create))
        {
            await arquivo.CopyToAsync(stream);
        }

        return Ok(new
        {
            mensagem = "Arquivo enviado com sucesso."
        });
    }
}