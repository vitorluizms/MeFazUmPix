using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWallet.Models
{

    public enum PaymentStatus
    {
        Pendent,
        Approved,
        Denied
    }
    public class Payments
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int PixKeyId { get; set; }

        [Required]
        public int PaymentProviderId { get; set; }

        [Required]
        public int Amount { get; set; }

        [EnumDataType(typeof(PaymentStatus))]
        public string Status { get; set; } = "Pendent";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public PixKeys? PixKeys { get; set; }
        public PaymentProvider? PaymentProvider { get; set; }
    }
}