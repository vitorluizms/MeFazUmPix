namespace MyWallet.Exceptions;

public class NotFoundError : Exception
{
    public NotFoundError(string message) : base(message)
    {
    }
}