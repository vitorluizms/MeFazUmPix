using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
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

public partial record class KeyDTO : IValidatableObject
{
    private static readonly Dictionary<string, string> ValidationPatterns = new()
    {
        ["CPF"] = @"^\d{3}\.\d{3}\.\d{3}-\d{2}$",
        ["Email"] = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$",
        ["Phone"] = @"^\(\d{2}\)\s9\s\d{4}-\d{4}$"
    };

    [Required]
    public required string Value { get; set; }

    [Required]
    [RegularExpression("^(CPF|Phone|Email|Random)$", ErrorMessage = "Invalid Type. Must be CPF, Phone, Email or Random.")]
    public required string Type { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!ValidationPatterns.TryGetValue(Type, out var regexPattern))
        {
            yield return new ValidationResult("Invalid Type.", [nameof(Type)]);
            yield break;
        }

        if (!Regex.IsMatch(Value, regexPattern))
        {
            yield return new ValidationResult($"Invalid format for {Type}.", [nameof(Value)]);
        }

    }
}

public record class UserDTO
{
    [Required]
    [StringLength(14)]
    [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "Invalid CPF. Must be in the format 000.000.000-00.")]
    public required string CPF { get; set; }
}

public record class AccountDTO
{
    [Required]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "Field Number of Account must have 8 digits .")]
    public required int Number { get; set; }

    [Required]
    [RegularExpression(@"^\d{4}$", ErrorMessage = "Field Number of Account must have 4 digits .")]
    public required int Agency { get; set; }
}