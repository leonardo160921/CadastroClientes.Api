using CadastroClientes.Api.Models;

namespace CadastroClientes.Api.Services
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> ObterTodos(
            int page,
            int pageSize);

        Task<Cliente?> ObterPorId(int id);

        Task Adicionar(Cliente cliente);

        Task Atualizar(Cliente cliente);

        Task Excluir(Cliente cliente);
        Task<Cliente?> ObterPorCPF(string cpf);
    }
}