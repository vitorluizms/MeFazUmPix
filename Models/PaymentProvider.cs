using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWallet.Models
{
    public class PaymentProvider
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        public required string Token { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now.ToUniversalTime();
        public DateTime? UpdatedAt { get; set; }

        public ICollection<PixKeys>? PixKeys { get; set; }
        public ICollection<Account>? Accounts { get; set; }
        public ICollection<Payments>? Payments { get; set; }
    }
}
