using System.ComponentModel.DataAnnotations;

namespace CadastroClientes.Api.DTOs
{
    public class CriarClienteDTO
    {
        public string Nome { get; set; }

        public string CPF { get; set; }

        public string Email { get; set; }

        public string Telefone { get; set; }

        public DateTime DataNascimento { get; set; }
    }
}