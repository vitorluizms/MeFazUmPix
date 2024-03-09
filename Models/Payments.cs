using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWallet.Models
{
    public class Payments
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int AccountId { get; set; }

        [Required]
        public int PixKeyId { get; set; }

        [Required]
        public int PaymentProviderId { get; set; }

        [Required]
        public int Amount { get; set; }
        public string Status { get; set; } = "Pendent";

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public User? User { get; set; }
        public Account? Account { get; set; }
        public PixKeys? PixKeys { get; set; }
        public PaymentProvider? PaymentProvider { get; set; }
    }
}