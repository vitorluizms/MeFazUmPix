namespace MyWallet.Models;
public class BaseEntity
{
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}