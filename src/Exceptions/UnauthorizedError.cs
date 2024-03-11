namespace MyWallet.Exceptions;

public class UnauthorizedError : Exception
{
    public UnauthorizedError(string message) : base(message)
    {
    }
}