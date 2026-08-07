using CadastroClientes.Api.Models;

namespace CadastroClientes.Api.Repositories;

public interface IClienteRepository
{
    Task<IEnumerable<Cliente>> ObterTodos(int page, int pageSize);

    Task<Cliente?> ObterPorId(int id);

    Task<Cliente?> ObterPorCPF(string cpf);

    Task Adicionar(Cliente cliente);

    Task Atualizar(Cliente cliente);

    Task Excluir(Cliente cliente);
}