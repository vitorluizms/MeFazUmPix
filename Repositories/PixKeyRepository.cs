using Microsoft.EntityFrameworkCore;
using MyWallet.Data;
using MyWallet.Models;

namespace MyWallet.Repositories;

public class KeyRepository
{

    private readonly AppDbContext _context;

    public KeyRepository(AppDbContext context)
    {
        _context = context;
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

    public async Task<PixKeys?> GetPixKeyByValue(string type, string value)
    {
        return await _context.PixKeys
            .Include(Account => Account != null ? Account.PaymentProvider : null)
            .Include(pk => pk.Account)
                .ThenInclude(account => account != null ? account.User : null)
            .FirstOrDefaultAsync(pk => pk.Type.Equals(type) && pk.Value.Equals(value));
    }
}