using Microsoft.EntityFrameworkCore;
using MyWallet.Data;
using MyWallet.Models;

namespace MyWallet.Repositories
{
    public class UserRepository(AppDbContext context)
    {

        private readonly AppDbContext _context = context;

        public async Task<Users?> GetUserByCPF(string cpf)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.CPF.Equals(cpf));
        }
    }
}