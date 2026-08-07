using CadastroClientes.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CadastroClientes.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Cliente> Clientes { get; set; }
}