using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;

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

        await using var channel = await connection.CreateChannelAsync();
        await channel.ExchangeDeclareAsync(
            exchange: "clientes",
            type: ExchangeType.Direct,
            durable: true,
            autoDelete: false);

        await channel.QueueDeclareAsync(
            queue: "clientes",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        await channel.QueueBindAsync(
            queue: "clientes",
            exchange: "clientes",
            routingKey: "cliente.criado");

        var body = Encoding.UTF8.GetBytes(mensagem);

        await channel.BasicPublishAsync(
                exchange: "clientes",
                routingKey: "cliente.criado",
                body: body);


        Console.WriteLine("Conexão com RabbitMQ estabelecida.");
        Console.WriteLine("Channel criado com sucesso.");
        Console.WriteLine("Exchange criada com sucesso.");
        Console.WriteLine("Queue criada com sucesso.");
        Console.WriteLine("Binding criado com sucesso.");
    }
}