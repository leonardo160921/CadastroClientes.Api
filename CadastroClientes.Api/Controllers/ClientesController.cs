using Asp.Versioning;
using CadastroClientes.Api.DTOs;
using CadastroClientes.Api.Models;
using CadastroClientes.Api.Options;
using CadastroClientes.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using AutoMapper;

namespace CadastroClientes.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiExplorerSettings(GroupName = "v1")]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _service;
    private readonly ILogger<ClientesController> _logger;
    private readonly SistemaOptions _sistema;
    private readonly IMapper _mapper;


    public ClientesController(
    IClienteService service,
    ILogger<ClientesController> logger,
    IOptions<SistemaOptions> options,
    IMapper mapper)
    {
        _service = service;
        _logger = logger;
        _sistema = options.Value;
        _mapper = mapper;
    }

    [HttpGet]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> ObterTodos([FromQuery] PaginacaoDTO paginacao)
    {
        var clientes = await _service.ObterTodos(
            paginacao.Page,
            paginacao.PageSize);

        var clientesDTO = _mapper.Map<IEnumerable<ClienteDTO>>(clientes);

        return Ok(clientesDTO);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        _logger.LogInformation("Consultando cliente {Id}.", id);

        var cliente = await _service.ObterPorId(id);

        if (cliente == null)
        {
            _logger.LogWarning("Cliente {Id} não encontrado.", id);
            return NotFound();
        }

        var dto = _mapper.Map<ClienteDTO>(cliente);

        return Ok(dto);

    }

    [HttpGet("configuracao")]
    public IActionResult Configuracao()
    {
        return Ok(new
        {
            _sistema.Nome,
            _sistema.Versao,
            _sistema.Empresa
        });
    }

    [HttpPost]
    public async Task<IActionResult> Cadastrar([FromBody] CriarClienteDTO dto)
    {
        _logger.LogInformation("Cadastrando novo cliente.");

        var cliente = _mapper.Map<Cliente>(dto);

        cliente.Ativo = true;

        await _service.Adicionar(cliente);

        var clienteDTO = _mapper.Map<ClienteDTO>(cliente);

        _logger.LogInformation("Cliente {Id} cadastrado com sucesso.", cliente.Id);

        return CreatedAtAction(
            nameof(ObterPorId),
            new { id = cliente.Id },
            clienteDTO);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarClienteDTO dto)
    {
        _logger.LogInformation("Atualizando cliente {Id}.", id);

        var cliente = await _service.ObterPorId(id);

        if (cliente == null)
        {
            _logger.LogWarning("Cliente {Id} não encontrado.", id);
            return NotFound();
        }

        _mapper.Map(dto, cliente);

        await _service.Atualizar(cliente);

        _logger.LogInformation("Cliente {Id} atualizado com sucesso.", id);

        var clienteDTO = _mapper.Map<ClienteDTO>(cliente);

        return Ok(clienteDTO);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Excluir(int id)
    {
        _logger.LogInformation("Excluindo cliente {Id}.", id);

        var cliente = await _service.ObterPorId(id);

        if (cliente == null)
        {
            _logger.LogWarning("Cliente {Id} não encontrado.", id);
            return NotFound();
        }

        await _service.Excluir(cliente);

        _logger.LogInformation("Cliente {Id} excluído com sucesso.", id);

        return NoContent();
    }
}