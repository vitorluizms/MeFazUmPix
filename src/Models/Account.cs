using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWallet.Models
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int PaymentProviderId { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        public int Agency { get; set; }

        public Users? User { get; set; }

        public PaymentProvider? PaymentProvider { get; set; }

        public ICollection<PixKeys>? PixKeys { get; }

        [DefaultValue("Now()")]
        public DateTime CreatedAt { get; set; } = DateTime.Now.ToUniversalTime();
        public DateTime? UpdatedAt { get; set; }
    }
}
