using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWallet.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(14)] // CPF length
        public required string CPF { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; }

        public ICollection<Account>? Accounts { get; set; }
        public ICollection<Payments>? Payments { get; set; }
    }
}