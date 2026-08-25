namespace CadastroClientes.Api.Messaging;

public interface IRabbitMqService
{
    Task PublicarAsync(string mensagem);
}