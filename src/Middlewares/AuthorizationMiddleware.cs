using MyWallet.Exceptions;
using MyWallet.Models;
using MyWallet.Repositories;

namespace MyWallet.Middlewares;

public class AuthorizationMiddleware
{

    private readonly PaymentProviderRepository _paymentProviderRepository;
    public AuthorizationMiddleware(PaymentProviderRepository paymentProviderRepository)
    {
        _paymentProviderRepository = paymentProviderRepository;
    }

    public async Task<int> ValidatePSPToken(string token)
    {

        if (string.IsNullOrEmpty(token)) throw new UnauthorizedError("PSP Token is missing");

        PaymentProvider? paymentProvider = await _paymentProviderRepository.GetPaymentProviderByToken(token) ?? throw new UnauthorizedError("Payment Provider not found");

        return paymentProvider.Id;
    }
}
