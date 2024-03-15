using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using MyWallet.Entities;

namespace MyWallet.DTOs;

public class RequestBody
{
    public KeyDTO? Key { get; set; }
    public UserDTO? User { get; set; }
    public AccountDTO? Account { get; set; }

}

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
        if (!ValidationPatterns.TryGetValue(Type, out var regexPattern) && Type != "Random")
        {
            yield return new ValidationResult("Invalid Type.", [nameof(Type)]);
            yield break;
        }

        if (Type != "Random" && regexPattern != null && !Regex.IsMatch(Value, regexPattern))
        {
            yield return new ValidationResult($"Invalid format for {Type}.", [nameof(Value)]);
        }

    }
}

public record class UserDTO
{
    [Required(ErrorMessage = "CPF is required.")]
    [StringLength(14, ErrorMessage = "CPF must have 14 characters.")]
    [RegularExpression(@"^(0\d{2}|[1-9]\d{2})\.(0\d{2}|[1-9]\d{2})\.(0\d{2}|[1-9]\d{2})-(0\d|1\d|2\d|3[0-2])$", ErrorMessage = "Invalid CPF. Must be in the format 000.000.000-00.")]
    public required string CPF { get; set; }
}


public partial record class AccountDTO
{
    [Required(ErrorMessage = "Number of Account is required.")]
    public required int Number { get; set; }

    [Required(ErrorMessage = "Agency is required.")]
    public required int Agency { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var numberString = Number.ToString();
        var agencyString = Agency.ToString();

        if (!MyRegex().IsMatch(numberString))
        {
            yield return new ValidationResult("Field Number of Account must have up to 8 digits with optional leading zeros.", new[] { nameof(Number) });
        }

        if (!MyRegex1().IsMatch(agencyString))
        {
            yield return new ValidationResult("Field Agency must have up to 4 digits with optional leading zeros.", new[] { nameof(Agency) });
        }
    }

    [GeneratedRegex(@"^0*\d{1,8}$")]
    private static partial Regex MyRegex();
    [GeneratedRegex(@"^0*\d{1,4}$")]
    private static partial Regex MyRegex1();
}
