using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using MyWallet.Models;

namespace MyWallet.Data;

public class AppDbContext(DbContextOptions<AppDbContext> Options) : DbContext(Options)
{
    public DbSet<Users> Users { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<PaymentProvider> PaymentProviders { get; set; }
    public DbSet<PixKeys> PixKeys { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        // Validation - User CPF is Unique
        modelBuilder.Entity<Users>()
            .HasIndex(u => u.CPF)
            .IsUnique();

        modelBuilder.Entity<PixKeys>()
            .HasOne(pk => pk.Account)
            .WithMany(a => a.PixKeys)
            .HasForeignKey(pk => pk.AccountId)
            .IsRequired()
            .HasConstraintName("FK_PixKeys_Account");

        modelBuilder.Entity<Account>()
            .HasIndex(a => new { a.Number, a.Agency })
            .IsUnique();

        // Validation: Relationship between Account and User
        modelBuilder.Entity<Account>()
            .HasOne(a => a.User)
            .WithMany(u => u.Accounts)
            .HasForeignKey(a => a.UserId)
            .IsRequired()
            .HasConstraintName("FK_Account_User");

        // Validation: Relationship between Account and PaymentProvider
        modelBuilder.Entity<Account>()
            .HasOne(a => a.PaymentProvider)
            .WithMany(pp => pp.Accounts)
            .HasForeignKey(a => a.PaymentProviderId)
            .IsRequired()
            .HasConstraintName("FK_Account_PaymentProvider");

        // Validation: Relationship between Payment and PixKey
        modelBuilder.Entity<Payments>()
            .HasOne(p => p.PixKeys)
            .WithMany(pk => pk.Payments)
            .HasForeignKey(p => p.PixKeyId)
            .IsRequired()
            .HasConstraintName("FK_Payment_PixKey");

        // Validation: Relationship between Payment and PaymentProvider
        modelBuilder.Entity<Payments>()
            .HasOne(p => p.PaymentProvider)
            .WithMany(pp => pp.Payments)
            .HasForeignKey(p => p.PaymentProviderId)
            .IsRequired()
            .HasConstraintName("FK_Payment_PaymentProvider");

        // Validation: Each PixKey must be unique:
        modelBuilder.Entity<PixKeys>()
            .HasIndex(pk => pk.Value)
            .IsUnique();

        // Validation: Only one PIX key can exist:
        modelBuilder.Entity<PixKeys>()
            .HasIndex(pk => pk.Value)
            .IsUnique();

        // Set the default value for the CreatedAt field

        modelBuilder.Entity<Users>()
            .Property(u => u.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

        modelBuilder.Entity<Account>()
            .Property(a => a.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

        modelBuilder.Entity<PaymentProvider>()
            .Property(p => p.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

        modelBuilder.Entity<PixKeys>()
            .Property(pk => pk.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

        modelBuilder.Entity<Payments>()
            .Property(p => p.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");
    }

}