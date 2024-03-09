using Microsoft.EntityFrameworkCore;

namespace MyWallet.Data;

public class AppDbContext(DbContextOptions<AppDbContext> Options) : DbContext(Options)
{

}