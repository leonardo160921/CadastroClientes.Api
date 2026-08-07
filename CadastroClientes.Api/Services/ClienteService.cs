using CadastroClientes.Api.Models;
using CadastroClientes.Api.Repositories;

namespace CadastroClientes.Api.Services;

public class ClienteService : IClienteService
{
    private readonly IClienteRepository _repository;
    private readonly ICacheService _cacheService;
    private readonly INotificacaoService _notificacaoService;

    public ClienteService(
        IClienteRepository repository,
        ICacheService cacheService,
        INotificacaoService notificacaoService)
    {
        _repository = repository;
        _cacheService = cacheService;
        _notificacaoService = notificacaoService;
    }

    public async Task<IEnumerable<Cliente>> ObterTodos(int page, int pageSize)
    {
        const string cacheKey = "clientes";

        var clientes = _cacheService.Obter<IEnumerable<Cliente>>(cacheKey);

        if (clientes != null)
        {
            return clientes;
        }

        clientes = await _repository.ObterTodos(page, pageSize);

        _cacheService.Salvar(
            cacheKey,
            clientes,
            TimeSpan.FromMinutes(5));

        return clientes;
    }

    public async Task<Cliente?> ObterPorId(int id)
    {
        return await _repository.ObterPorId(id);
    }
 
    public async Task Adicionar(Cliente cliente)
    {
        var clienteExistente =
       await _repository.ObterPorCPF(cliente.CPF);

        if (clienteExistente != null)
        {
            throw new InvalidOperationException("Já existe um cliente com este CPF.");
        }

        await _repository.Adicionar(cliente);

        _cacheService.Remover("clientes");

        await _notificacaoService.EnviarBoasVindas(cliente);
    }
    public async Task<Cliente?> ObterPorCPF(string cpf)
    {
        return await _repository.ObterPorCPF(cpf);
    }

    public async Task Atualizar(Cliente cliente)
    {
        await _repository.Atualizar(cliente);

        _cacheService.Remover("clientes");
    }

    public async Task Excluir(Cliente cliente)
    {
        await _repository.Excluir(cliente);

        _cacheService.Remover("clientes");
    }
   
}