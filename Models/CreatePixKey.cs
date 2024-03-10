namespace MyWallet.Entities
{
    public class PixKeyCreationData
    {
        public required string Type { get; set; } // CPF, Email, Phone, Random
        public required string Value { get; set; }
        public required string CPF { get; set; }
        public required int Number { get; set; }
        public required int Agency { get; set; }
    }
}