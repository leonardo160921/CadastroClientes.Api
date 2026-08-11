using CadastroClientes.Api.Models;
using CadastroClientes.Api.Repositories;
using CadastroClientes.Api.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace CadastroClientes.Tests.Services;

public class ClienteServiceTests
{
    [Fact]
    public void Deve_Criar_Service()
    {
        var repository = new Mock<IClienteRepository>();
        var cache = new Mock<ICacheService>();
        var notificacao = new Mock<INotificacaoService>();

        var service = new ClienteService(
            repository.Object,
            cache.Object,
            notificacao.Object);

        service.Should().NotBeNull();
       
    }
    [Fact]
    public async Task Deve_Chamar_Repository_Ao_Adicionar()
    {
        var repository = new Mock<IClienteRepository>();
        var cache = new Mock<ICacheService>();
        var notificacao = new Mock<INotificacaoService>();

        var service = new ClienteService(
            repository.Object,
            cache.Object,
            notificacao.Object);

        var cliente = new Cliente
        {
            Nome = "Leonardo",
            CPF = "12345678901",
            Email = "leo@email.com"
        };

        await service.Adicionar(cliente);

        repository.Verify(
            x => x.Adicionar(cliente),
            Times.Once);
    }
    [Fact]
    public async Task Deve_Retornar_Clientes_Do_Cache()
    {
        // Arrange
        var repository = new Mock<IClienteRepository>();
        var cache = new Mock<ICacheService>();
        var notificacao = new Mock<INotificacaoService>();

        var lista = new List<Cliente>
    {
        new Cliente
        {
            Id = 1,
            Nome = "Leonardo"
        }
    };

        cache.Setup(x => x.Obter<IEnumerable<Cliente>>("clientes"))
             .Returns(lista);

        var service = new ClienteService(
            repository.Object,
            cache.Object,
            notificacao.Object);

        // Act
        var resultado = await service.ObterTodos(1, 10);

        // Assert
        resultado.Should().HaveCount(1);

        repository.Verify(
            x => x.ObterTodos(It.IsAny<int>(), It.IsAny<int>()),
            Times.Never);
    }
    [Fact]
    public async Task Deve_Buscar_Do_Repositorio_Quando_Cache_Estiver_Vazio()
    {
        // Arrange
        var repository = new Mock<IClienteRepository>();
        var cache = new Mock<ICacheService>();
        var notificacao = new Mock<INotificacaoService>();

        cache.Setup(x => x.Obter<IEnumerable<Cliente>>("clientes"))
             .Returns((IEnumerable<Cliente>)null!);

        repository.Setup(x => x.ObterTodos(1, 10))
                  .ReturnsAsync(new List<Cliente>
                  {
                  new Cliente
                  {
                      Id = 1,
                      Nome = "Leonardo"
                  }
                  });

        var service = new ClienteService(
            repository.Object,
            cache.Object,
            notificacao.Object);

        // Act
        var resultado = await service.ObterTodos(1, 10);

        // Assert
        resultado.Should().HaveCount(1);

        repository.Verify(
            x => x.ObterTodos(1, 10),
            Times.Once);

        cache.Verify(
            x => x.Salvar(
                "clientes",
                It.IsAny<IEnumerable<Cliente>>(),
                It.IsAny<TimeSpan>()),
            Times.Once);
    }
}