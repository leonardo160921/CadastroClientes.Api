namespace CadastroClientes.Api.Services;

public interface ICacheService
{
    T? Obter<T>(string chave);

    void Salvar<T>(
        string chave,
        T valor,
        TimeSpan tempo);

    void Remover(string chave);
}