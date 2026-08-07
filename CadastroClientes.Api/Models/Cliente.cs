namespace CadastroClientes.Api.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string CPF { get; set;} = string.Empty;
        public string Email { get; set;} = string.Empty;
        public string Telefone { get; set;} = string.Empty;
        public DateTime DataNascimento { get; set; }
        public bool Ativo {  get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
