using Microsoft.EntityFrameworkCore;
using MyWallet.Data;
using MyWallet.Models;

namespace MyWallet.Repositories
{
    public class PaymentProviderRepository(AppDbContext context)
    {
        private readonly AppDbContext _context = context;

        public async Task<PaymentProvider?> GetPaymentProviderByToken(string token)
        {
            return await _context.PaymentProviders.FirstOrDefaultAsync(p => p.Token.Equals(token));
        }

    }
}