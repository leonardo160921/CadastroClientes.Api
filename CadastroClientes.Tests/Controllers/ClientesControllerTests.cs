using AutoMapper;
using CadastroClientes.Api.Controllers;
using CadastroClientes.Api.DTOs;
using CadastroClientes.Api.Models;
using CadastroClientes.Api.Options;
using CadastroClientes.Api.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace CadastroClientes.Tests.Controllers;

public class ClientesControllerTests
{
    private readonly Mock<IClienteService> _service;
    private readonly Mock<ILogger<ClientesController>> _logger;
    private readonly Mock<IMapper> _mapper;

    private readonly ClientesController _controller;

    public ClientesControllerTests()
    {
        _service = new Mock<IClienteService>();
        _logger = new Mock<ILogger<ClientesController>>();
        _mapper = new Mock<IMapper>();

        var options = Options.Create(new SistemaOptions
        {
            Nome = "Cadastro Clientes",
            Versao = "1.0",
            Empresa = "Curso ASP.NET Core"
        });

        _controller = new ClientesController(
            _service.Object,
            _logger.Object,
            options,
            _mapper.Object);
    }
    [Fact]
    public async Task ObterPorId_Deve_Retornar_Ok()
    {
        // Arrange
        var cliente = new Cliente
        {
            Id = 1,
            Nome = "Leonardo",
            Email = "leo@email.com"
        };

        var dto = new ClienteDTO
        {
            Id = 1,
            Nome = "Leonardo",
            Email = "leo@email.com"
        };

        _service.Setup(x => x.ObterPorId(1))
                .ReturnsAsync(cliente);

        _mapper.Setup(x => x.Map<ClienteDTO>(cliente))
               .Returns(dto);

        // Act
        var resultado = await _controller.ObterPorId(1);

        // Assert
        resultado.Should().BeOfType<OkObjectResult>();
    }
    [Fact]
    public async Task ObterPorId_Deve_Retornar_NotFound()
    {
        // Arrange
        _service.Setup(x => x.ObterPorId(1))
                .ReturnsAsync((Cliente?)null);

        // Act
        var resultado = await _controller.ObterPorId(1);

        // Assert
        resultado.Should().BeOfType<NotFoundResult>();
    }
    [Fact]
    public async Task Cadastrar_Deve_Retornar_CreatedAtAction()
    {
        // Arrange
        var dto = new CriarClienteDTO
        {
            Nome = "Leonardo",
            CPF = "12345678901",
            Email = "leo@email.com",
            Telefone = "11999999999",
            DataNascimento = new DateTime(1990, 1, 1)
        };

        var cliente = new Cliente
        {
            Id = 1,
            Nome = dto.Nome,
            CPF = dto.CPF,
            Email = dto.Email,
            Telefone = dto.Telefone,
            DataNascimento = dto.DataNascimento
        };

        var clienteDTO = new ClienteDTO
        {
            Id = 1,
            Nome = cliente.Nome,
            Email = cliente.Email
        };

        _mapper.Setup(x => x.Map<Cliente>(dto))
               .Returns(cliente);

        _mapper.Setup(x => x.Map<ClienteDTO>(cliente))
               .Returns(clienteDTO);

        // Act
        var resultado = await _controller.Cadastrar(dto);

        // Assert
        resultado.Should().BeOfType<CreatedAtActionResult>();
    }
    [Fact]
    public async Task Atualizar_Deve_Retornar_Ok()
    {
        // Arrange
        var dto = new AtualizarClienteDTO
        {
            Nome = "Leonardo Atualizado",
            CPF = "12345678901",
            Email = "leo@email.com",
            Telefone = "11999999999",
            DataNascimento = new DateTime(1990, 1, 1),
            Ativo = true
        };

        var cliente = new Cliente
        {
            Id = 1,
            Nome = "Leonardo"
        };

        var clienteDTO = new ClienteDTO
        {
            Id = 1,
            Nome = dto.Nome,
            Email = dto.Email
        };

        _service.Setup(x => x.ObterPorId(1))
                .ReturnsAsync(cliente);

        _mapper.Setup(x => x.Map(dto, cliente));

        _mapper.Setup(x => x.Map<ClienteDTO>(cliente))
               .Returns(clienteDTO);

        // Act
        var resultado = await _controller.Atualizar(1, dto);

        // Assert
        resultado.Should().BeOfType<OkObjectResult>();
    }
    [Fact]
    public async Task Excluir_Deve_Retornar_NoContent()
    {
        // Arrange
        var cliente = new Cliente
        {
            Id = 1,
            Nome = "Leonardo"
        };

        _service.Setup(x => x.ObterPorId(1))
                .ReturnsAsync(cliente);

        // Act
        var resultado = await _controller.Excluir(1);

        // Assert
        resultado.Should().BeOfType<NoContentResult>();
    }

}