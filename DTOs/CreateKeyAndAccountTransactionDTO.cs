namespace MyWallet.DTOs
{
    public class CreateKeyAndAccountTransactionDTO
    {
        public required string Type { get; set; }
        public required string Value { get; set; }
        public required int Number { get; set; }
        public required int Agency { get; set; }
        public required int PaymentProviderId { get; set; }
        public required int UserId { get; set; }
    }

}