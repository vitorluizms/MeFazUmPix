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
}