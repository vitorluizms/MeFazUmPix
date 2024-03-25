using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using MyWallet.Entities;
using MyWallet.Models;
using MyWallet.Utils;

namespace MyWallet.DTOs;

public class GetKeyByValueDTO
{
    [Required]
    [RegularExpression("^(CPF|Phone|Email|Random)$", ErrorMessage = "Invalid Type. Must be CPF, Phone, Email or Random.")]
    public required string Type { get; set; }

    [Required]
    public required string Value { get; set; }

    public PixKeyGetByValueData ToEntity()
    {
        return new PixKeyGetByValueData
        {
            Type = Type,
            Value = Value
        };
    }


    public ResponseBody ConvertDataToGetKeyByValue(PixKeys key)
    {
        return new ResponseBody
        {
            Key = new Key
            {
                Value = key.Value,
                Type = key.Type
            },
            User = new User
            {
                CPF = CpfUtil.ToMasked(key.Account?.User?.CPF)
            },
            Account = new Account
            {
                Number = key.Account?.Number,
                Agency = key.Account?.Agency,
                BankName = key.PaymentProvider?.Name,
                BankId = key.PaymentProvider?.Id
            }
        };
    }

    public class ResponseBody
    {
        public Key? Key { get; set; }
        public User? User { get; set; }
        public Account? Account { get; set; }
    }

    public class Key
    {
        public required string Value { get; set; }
        public required string Type { get; set; }
    }

    public class User
    {
        public required string? CPF { get; set; }
    }

    public class Account
    {
        public required int? Number { get; set; }
        public required int? Agency { get; set; }
        public required string? BankName { get; set; }
        public required int? BankId { get; set; }
    }

}