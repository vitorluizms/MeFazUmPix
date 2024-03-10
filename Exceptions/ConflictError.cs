namespace MyWallet.Exceptions;

public class ConflictError : Exception
{
    public ConflictError(string message) : base(message)
    {
    }
}