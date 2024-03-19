using MyWallet.DTOs;
using MyWallet.Exceptions;
using MyWallet.Models;
using MyWallet.Repositories;

namespace MyWallet.Services;

public class PaymentsService
{
    private readonly KeysService _keysService;
    private readonly AccountRepository _accountRepository;
    private readonly PaymentsRepository _paymentsRepository;

    public PaymentsService(KeysService keysService, AccountRepository accountRepository, PaymentsRepository paymentsRepository)
    {
        _keysService = keysService;
        _accountRepository = accountRepository;
        _paymentsRepository = paymentsRepository;
    }

    readonly int MIN_PAYMENT_INTERVAL = 30;

    public async Task CreatePayment(PaymentsCreationData dto, PaymentProvider paymentProvider)
    {
        Users user = await _keysService.ValidateUser(dto.OriginUserCPF);
        Account account = await _accountRepository.GetAccountByNumberAndAgency(dto.OriginAccountNumber, dto.OriginAccountAgency) ?? throw new NotFoundError ("Account not found");

        if (account.UserId != user.Id) throw new UnauthorizedError("User is not the owner of the origin account");

        PixKeys key = await _keysService.GetKeyByValue(dto.DestinyKeyValue, dto.DestinyKeyType);    

        ValidateSelfPayment(account, key);

        PaymentIdempotenceKey idempotenceKey = new(dto.Amount, key.Id, account.PaymentProviderId); 
        ValidateDuplicatedPayment(idempotenceKey);

    }

    private static void ValidateSelfPayment(Account account, PixKeys key)
    {
        if (account.Id == key.AccountId) throw new BadRequestError("Self payment");
    }

    private async void ValidateDuplicatedPayment(PaymentIdempotenceKey key)
    {
        Payments? payments = await _paymentsRepository.GetPaymentByIdempotenceKey(key, MIN_PAYMENT_INTERVAL);

        if (payments != null) throw new BadRequestError("Duplicated payment");
    }
}   