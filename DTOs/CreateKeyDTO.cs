using System.ComponentModel.DataAnnotations;
using MyWallet.Entities;

namespace MyWallet.DTOs;

public class CreateKeyDTO
{
    public required KeyDTO Key { get; set; }
    public required UserDTO User { get; set; }
    public required AccountDTO Account { get; set; }

    public PixKeyCreationData ToEntity()
    {
        return new PixKeyCreationData
        {
            Type = Key.Type,
            Value = Key.Value,
            CPF = User.CPF,
            Number = Account.Number,
            Agency = Account.Agency
        };
    }
}

public record class KeyDTO
{
    [Required]
    public required string Value { get; set; }

    [Required]
    [RegularExpression("^(CPF|Phone|Email|Random)$", ErrorMessage = "Tipo inválido.")]
    public required string Type { get; set; }
}

public record class UserDTO
{
    [Required]
    [StringLength(14)]
    public required string CPF { get; set; }
}

public record class AccountDTO
{
    [Required]
    public required string Number { get; set; }
    [Required]
    public required string Agency { get; set; }
}