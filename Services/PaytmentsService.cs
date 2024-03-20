using MyWallet.Dtos;
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
    private readonly MessageService _messageService;

    public PaymentsService(KeysService keysService, AccountRepository accountRepository, PaymentsRepository paymentsRepository, MessageService messageService)
    {
        _keysService = keysService;
        _accountRepository = accountRepository;
        _paymentsRepository = paymentsRepository;
        _messageService = messageService;
    }

    readonly int MIN_PAYMENT_INTERVAL = 30;

    public async Task<int> CreatePayment(CreatePaymentDTO dto, PaymentProvider paymentProvider)
    {
        Users user = await _keysService.ValidateUser(dto.ToEntity().OriginUserCPF);
        Account account = await _accountRepository.GetAccountByNumberAndAgency(dto.ToEntity().OriginAccountNumber, dto.ToEntity().OriginAccountAgency) ?? throw new NotFoundError ("Account not found");

        if (account.UserId != user.Id) throw new UnauthorizedError("User is not the owner of the origin account");

        PixKeys key = await _keysService.GetKeyByValue(dto.ToEntity().DestinyKeyType, dto.ToEntity().DestinyKeyValue);    

        ValidateSelfPayment(account, key);

        PaymentIdempotenceKey idempotenceKey = new(dto.Amount, key.Id, account.Id); 
        await ValidateDuplicatedPayment(idempotenceKey);

        Payments payment = await _paymentsRepository.CreatePayment(dto.PaymentToEntity(key.Id, account.Id));
        PaymentMessageDTO message = new(payment.Id, dto);

        _messageService.SendMessage(message);

        return payment.Id;

    }

    private static void ValidateSelfPayment(Account account, PixKeys key)
    {
        if (account.Id == key.AccountId) throw new BadRequestError("Self payment is not allowed");
    }

    private async Task ValidateDuplicatedPayment(PaymentIdempotenceKey key)
    {
        Payments? payments = await _paymentsRepository.GetPaymentByIdempotenceKey(key, MIN_PAYMENT_INTERVAL);
        Console.WriteLine($"Payments: {payments?.Id}");

        if (payments != null) throw new BadRequestError("Duplicated payment");
    }

    public async Task UpdatePaymentStatus(int id, string status)
    {
        Payments payment = await _paymentsRepository.GetPaymentById(id) ?? throw new NotFoundError("Payment not found");
        payment.Status = status;
        
        await _paymentsRepository.UpdatePayment(payment);
    }
}   