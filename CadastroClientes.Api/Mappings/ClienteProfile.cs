using AutoMapper;
using CadastroClientes.Api.DTOs;
using CadastroClientes.Api.Models;

namespace CadastroClientes.Api.Mappings;

public class ClienteProfile : Profile
{
    public ClienteProfile()
    {
        CreateMap<CriarClienteDTO, Cliente>();

        CreateMap<AtualizarClienteDTO, Cliente>();

        CreateMap<Cliente, ClienteDTO>();
    }
}