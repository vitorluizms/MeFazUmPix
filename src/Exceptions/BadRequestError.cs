namespace MyWallet.Exceptions;

public class BadRequestError : Exception
{
    public BadRequestError(string message) : base(message)
    {
    }
}