using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using MyWallet.Models;

namespace MyWallet.Data;

public class AppDbContext(DbContextOptions<AppDbContext> Options) : DbContext(Options)
{
    public DbSet<User> User { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<PaymentProvider> PaymentProviders { get; set; }
    public DbSet<PixKeys> PixKeys { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        // Validation - User CPF is Unique
        modelBuilder.Entity<User>()
            .HasIndex(u => u.CPF)
            .IsUnique();

        modelBuilder.Entity<PixKeys>()
            .HasOne(pk => pk.Account)
            .WithMany(a => a.PixKeys)
            .HasForeignKey(pk => pk.AccountId)
            .IsRequired()
            .HasConstraintName("FK_PixKeys_Account");

        // Validation: A bank account (number, agency) only exists in one bank (payment provider):
        modelBuilder.Entity<Account>()
             .HasIndex(a => new { a.UserId, a.PaymentProviderId })
             .IsUnique();

        // Validation - Relationship between PixKeys and User
        modelBuilder.Entity<PixKeys>()
            .HasOne(pk => pk.User)
            .WithMany(u => u.PixKeys)
            .HasForeignKey(pk => pk.UserId)
            .IsRequired()
            .HasConstraintName("FK_PixKeys_User");

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

        // Validation: Relationship between Payment and User
        modelBuilder.Entity<Payments>()
            .HasOne(p => p.User)
            .WithMany(u => u.Payments)
            .HasForeignKey(p => p.UserId)
            .IsRequired()
            .HasConstraintName("FK_Payment_User");

        // Validation: Relationship between Payment and Account
        modelBuilder.Entity<Payments>()
            .HasOne(p => p.Account)
            .WithMany(a => a.Payments)
            .HasForeignKey(p => p.AccountId)
            .IsRequired()
            .HasConstraintName("FK_Payment_Account");

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
    }

}