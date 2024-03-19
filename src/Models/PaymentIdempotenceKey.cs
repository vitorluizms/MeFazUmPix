using MyWallet.DTOs;

namespace MyWallet.Models
{
    public class PaymentIdempotenceKey(int amount, int pixKeyId, int paymentProviderAccountId)
    {
        public int PixKeyId { get; } = pixKeyId;
        public int PaymentProviderAccountId { get; } = paymentProviderAccountId;    
        public int Amount { get; } = amount;    
    }
}