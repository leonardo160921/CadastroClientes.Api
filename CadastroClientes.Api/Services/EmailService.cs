using CadastroClientes.Api.Models;

namespace CadastroClientes.Api.Services;

public class EmailService : INotificacaoService
{
    public Task EnviarBoasVindas(Cliente cliente)
    {
        Console.WriteLine($"E-mail de boas-vindas enviado para {cliente.Email}");

        return Task.CompletedTask;
    }
}