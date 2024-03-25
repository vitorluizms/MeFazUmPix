using MyWallet.Exceptions;

namespace MyWallet.Utils;

public static class CpfUtil
{
    public static string ToMasked(string? cpf)
    {
        if (string.IsNullOrEmpty(cpf))
        {
            throw new BadRequestError("Cpf cannot be null or empty");
        }

        return string.Concat(cpf.ToString().AsSpan(0, 3), ".XXX.XXX-", cpf.AsSpan(9, 2));
    }
}