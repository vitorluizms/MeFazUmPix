using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MyWallet.Data;
using MyWallet.DTOs;
using MyWallet.Exceptions;
using MyWallet.Models;

namespace MyWallet.Repositories;

public class KeyRepository
{

    private readonly AppDbContext _context;
    private readonly AccountRepository _accountRepository;

    public KeyRepository(AppDbContext context, AccountRepository accountRepository)
    {
        _context = context;
        _accountRepository = accountRepository;
    }

    public async Task<List<PixKeys>> GetPixKeyByUserId(int id)
    {
        return await _context.PixKeys.Where(pk => pk.Account != null && pk.Account.UserId.Equals(id)).ToListAsync();
    }

    public async Task<List<PixKeys>> AccountWithFiveOrMorePixKeys(int number, int agency)
    {
        return await _context.PixKeys.Where(pk => pk.Account != null && pk.Account.Number.Equals(number) && pk.Account.Agency.Equals(agency)).ToListAsync();
    }

    public async Task<PixKeys> CreateKey(string type, string value, int accountId, int paymentProviderId)
    {
        Console.WriteLine("Creating key");
        PixKeys pixKey = new()
        {
            Type = type,
            Value = value,
            AccountId = accountId,
            PaymentProviderId = paymentProviderId
        };

        _context.PixKeys.Add(pixKey);
        await _context.SaveChangesAsync();
        return pixKey;
    }

    public async Task<PixKeys> CreateKeyAndAccountTransaction(CreateKeyAndAccountTransactionDTO dto)
    {
        using var transaction = _context.Database.BeginTransaction();

        try
        {
            Account account = await _accountRepository.CreateAccount(dto.UserId, dto.Number, dto.Agency, dto.PaymentProviderId);

            PixKeys key = await CreateKey(dto.Type, dto.Value, account.Id, dto.PaymentProviderId);

            transaction.Commit();
            return key;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new BadRequestError(ex.InnerException?.Message ?? ex.Message);
        }
    }

    public async Task<PixKeys?> GetOnlyPixKeyByValue(string value)
    {
        return await _context.PixKeys.FirstOrDefaultAsync(pk => pk.Value.Equals(value));
    }

    public async Task<PixKeys?> GetPixKeyByValue(string type, string value)
    {
        return await _context.PixKeys
            .Include(Account => Account != null ? Account.PaymentProvider : null)
            .Include(pk => pk.Account)
                .ThenInclude(account => account != null ? account.User : null)
            .FirstOrDefaultAsync(pk => pk.Type.Equals(type) && pk.Value.Equals(value));
    }
}