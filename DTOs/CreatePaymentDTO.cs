using System.ComponentModel.DataAnnotations;
using MyWallet.Models;

namespace MyWallet.DTOs
{
    public class CreatePaymentDTO
    {
        public required OriginDTO Origin { get; set; }
        public required DestinyDTO Destiny { get; set; }
        public required int Amount { get; set; }
        public string? Description { get; set; }

        public PaymentsCreationData ToEntity()
        {
            return new PaymentsCreationData
            {
                OriginUserCPF = Origin.User.CPF,
                OriginAccountNumber = Origin.Account.Number,
                OriginAccountAgency = Origin.Account.Agency,
                DestinyKeyValue = Destiny.Key.Value,
                DestinyKeyType = Destiny.Key.Type,
                Amount = Amount,
                Description = Description ?? string.Empty
            };
        }
        
        public Payments PaymentToEntity(int PixKeyId, int PaymentProviderId)
        {
            return new Payments
            {
                Amount = Amount,
                Description = Description ?? string.Empty,
                PixKeyId = PixKeyId,
                PaymentProviderAccountId = PaymentProviderId,
            };
        }
    }

    public class OriginDTO
    {
        public required UserDTO User { get; set; }
        public required AccountDTO Account { get; set; }
    }

    public class DestinyDTO
    {
        public required KeyDTO Key { get; set; }
    }
}

namespace MyWallet.DTOs
{
    public class PaymentsCreationData
    {
        public required string OriginUserCPF { get; set; }
        public required int OriginAccountNumber { get; set; }
        public required int OriginAccountAgency { get; set; }
        public required string DestinyKeyValue { get; set; }
        public required string DestinyKeyType { get; set; }
        public required int Amount { get; set; }
        public required string Description { get; set; }
    }
}