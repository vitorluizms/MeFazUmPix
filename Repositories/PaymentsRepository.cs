using Microsoft.EntityFrameworkCore;
using MyWallet.Data;
using MyWallet.DTOs;
using MyWallet.Models;

namespace MyWallet.Repositories
{
    public class PaymentsRepository
    {
        private readonly AppDbContext _context;

        public PaymentsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Payments?> GetPaymentByIdempotenceKey(PaymentIdempotenceKey key, int seconds)
        {
            return await _context.Payments.Where(p =>
                p.PixKeyId == key.PixKeyId &&
                p.PaymentProviderAccountId == key.PaymentProviderAccountId &&
                p.Amount == key.Amount &&
                p.CreatedAt >= DateTime.UtcNow.AddSeconds(-seconds)
            ).FirstOrDefaultAsync();
        }

        public async Task<Payments> CreatePayment(Payments payment)
        {
            var entry = _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return entry.Entity;
        }
        public async Task<Payments?> GetPaymentById(int id)
        {
            return await _context.Payments.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task UpdatePayment(Payments payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
        }
    }
}
