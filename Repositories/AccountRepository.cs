using Microsoft.EntityFrameworkCore;
using MyWallet.Data;
using MyWallet.Models;

namespace MyWallet.Repositories
{
    public class AccountRepository(AppDbContext context)
    {
        private readonly AppDbContext _context = context;

        public async Task<List<Account>> GetAccountsByUserId(int id)
        {
            return await _context.Accounts.Where(a => a.UserId.Equals(id)).ToListAsync();
        }

        public async Task<Account?> GetAccountByNumber(int number)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Number.Equals(number));
        }   

        public async Task<Account?> GetAccountByNumberAndAgency(int number, int agency)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Number.Equals(number) && a.Agency.Equals(agency));
        }

        public async Task<Account> CreateAccount(int userId, int number, int agency, int paymentProviderId)
        {

            Account newAccount = new Account
            {
                UserId = userId,
                Number = number,
                Agency = agency,
                PaymentProviderId = paymentProviderId
            };

            _context.Accounts.Add(newAccount);
            await _context.SaveChangesAsync();

            return newAccount;
        }
    }
}