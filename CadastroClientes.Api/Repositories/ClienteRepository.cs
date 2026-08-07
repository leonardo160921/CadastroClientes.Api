using CadastroClientes.Api.Data;
using CadastroClientes.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CadastroClientes.Api.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly AppDbContext _context;

    public ClienteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Cliente>> ObterTodos(int page, int pageSize)
    {
        return await _context.Clientes
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Cliente?> ObterPorId(int id)
    {
        return await _context.Clientes.FindAsync(id);
    }

    public async Task Adicionar(Cliente cliente)
    {
        await _context.Clientes.AddAsync(cliente);

        await _context.SaveChangesAsync();
    }
    public async Task Atualizar(Cliente cliente)
    {
        _context.Clientes.Update(cliente);

        await _context.SaveChangesAsync();
    }

    public async Task Excluir(Cliente cliente)
    {
        _context.Clientes.Remove(cliente);

        await _context.SaveChangesAsync();
    }
    public async Task<Cliente?> ObterPorCPF(string cpf)
    {
        return await _context.Clientes
            .FirstOrDefaultAsync(x => x.CPF == cpf);
    }
}