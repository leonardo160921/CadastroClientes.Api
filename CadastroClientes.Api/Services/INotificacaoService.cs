using CadastroClientes.Api.Models;

namespace CadastroClientes.Api.Services;

public interface INotificacaoService
{
    Task EnviarBoasVindas(Cliente cliente);
}