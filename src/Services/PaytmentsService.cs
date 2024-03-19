using MyWallet.DTOs;
using MyWallet.Exceptions;
using MyWallet.Models;
using MyWallet.Repositories;

namespace MyWallet.Services;

public class PaymentsService
{
    private readonly KeysService _keysService;
    private readonly AccountRepository _accountRepository;

    public PaymentsService(KeysService keysService, AccountRepository accountRepository)
    {
        _keysService = keysService;
        _accountRepository = accountRepository;
    }

    public async Task CreatePayment(PaymentsCreationData dto, PaymentProvider paymentProvider)
    {
        Users user = await _keysService.ValidateUser(dto.OriginUserCPF);
        Account account = await _accountRepository.GetAccountByNumberAndAgency(dto.OriginAccountNumber, dto.OriginAccountAgency) ?? throw new NotFoundError ("Account not found");

        if (account.UserId != user.Id) throw new UnauthorizedError("User is not the owner of the origin account");

        PixKeys key = await _keysService.GetKeyByValue(dto.DestinyKeyValue, dto.DestinyKeyType);    
    }
}