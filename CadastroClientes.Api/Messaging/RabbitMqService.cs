using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace CadastroClientes.Api.Messaging;

public class RabbitMqService : IRabbitMqService
{
    private readonly RabbitMqOptions _options;

    public RabbitMqService(IOptions<RabbitMqOptions> options)
    {
        _options = options.Value;
    }

    public async Task PublicarAsync(string mensagem)
    {
        var factory = new ConnectionFactory
        {
            HostName = _options.HostName,
            Port = _options.Port,
            UserName = _options.UserName,
            Password = _options.Password
        };

        await using var connection = await factory.CreateConnectionAsync();

        Console.WriteLine("Conexão com RabbitMQ estabelecida.");
    }
}