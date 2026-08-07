using CadastroClientes.Api.DTOs;
using FluentValidation;

namespace CadastroClientes.Api.Validators;

public class CriarClienteDTOValidator : AbstractValidator<CriarClienteDTO>
{
    public CriarClienteDTOValidator()
    {
        RuleFor(x => x.Nome)
        .Cascade(CascadeMode.Stop)
        .NotEmpty().WithMessage("O nome é obrigatório.")
        .Length(3, 100)
        .WithMessage("O nome deve ter entre 3 e 100 caracteres.");

        RuleFor(x => x.CPF)
        .Cascade(CascadeMode.Stop)
        .NotEmpty().WithMessage("O CPF é obrigatório.")
        .Matches(@"^\d{11}$")
        .WithMessage("O CPF deve conter exatamente 11 dígitos.");

        RuleFor(x => x.Email)
        .Cascade(CascadeMode.Stop)
        .NotEmpty().WithMessage("O e-mail é obrigatório.")
        .EmailAddress()
        .WithMessage("O e-mail informado é inválido.");

        RuleFor(x => x.Telefone)
            .Matches(@"^\d{10,11}$")
            .When(x => !string.IsNullOrWhiteSpace(x.Telefone))
            .WithMessage("O telefone deve conter 10 ou 11 dígitos.");

        RuleFor(x => x.DataNascimento)
            .LessThan(DateTime.Today)
            .WithMessage("A data de nascimento deve ser anterior à data atual.");
    }
}