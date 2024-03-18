namespace MyWallet.Config;

public class QueueConfig
{
    public required string HostName { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required string QueueName { get; set; }
}