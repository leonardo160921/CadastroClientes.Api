using Microsoft.Extensions.Caching.Memory;

namespace CadastroClientes.Api.Services;

public class CacheService : ICacheService
{
    private readonly IMemoryCache _cache;

    public CacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public T? Obter<T>(string chave)
    {
        _cache.TryGetValue(chave, out T? valor);
        return valor;
    }

    public void Salvar<T>(string chave, T valor, TimeSpan tempo)
    {
        _cache.Set(chave, valor, tempo);
    }

    public void Remover(string chave)
    {
        _cache.Remove(chave);
    }
}