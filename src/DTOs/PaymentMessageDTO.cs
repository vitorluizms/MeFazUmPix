using MyWallet.DTOs;

namespace MyWallet.Dtos;

public class PaymentMessageDTO(
    int paymentId,
    CreatePaymentDTO createPaymentDTO)
{
        public int PaymentId { get; } = paymentId;
        public OriginDTO Origin { get; } = createPaymentDTO.Origin;
        public DestinyDTO Destiny { get; } = createPaymentDTO.Destiny;
        public int Amount { get; } = createPaymentDTO.Amount;
        public string? Description { get; } = createPaymentDTO.Description;
}