﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyWallet.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MyWallet.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240325032234_AddIndexToPaymentsCreatedAt")]
    partial class AddIndexToPaymentsCreatedAt
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MyWallet.Models.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Agency")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Number")
                        .HasColumnType("integer");

                    b.Property<int>("PaymentProviderId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PaymentProviderId");

                    b.HasIndex("UserId");

                    b.HasIndex("Number", "Agency")
                        .IsUnique();

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("MyWallet.Models.PaymentProvider", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Webhook")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Token")
                        .IsUnique();

                    b.ToTable("PaymentProviders");
                });

            modelBuilder.Entity("MyWallet.Models.Payments", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Amount")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int>("PaymentProviderAccountId")
                        .HasColumnType("integer");

                    b.Property<int>("PixKeyId")
                        .HasColumnType("integer");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("UsersId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CreatedAt");

                    b.HasIndex("PaymentProviderAccountId");

                    b.HasIndex("PixKeyId");

                    b.HasIndex("UsersId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("MyWallet.Models.PixKeys", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("PaymentProviderId")
                        .HasColumnType("integer");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("PaymentProviderId");

                    b.HasIndex("Value")
                        .IsUnique();

                    b.ToTable("PixKeys");
                });

            modelBuilder.Entity("MyWallet.Models.Users", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CPF")
                        .IsRequired()
                        .HasMaxLength(14)
                        .HasColumnType("character varying(14)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CPF")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MyWallet.Models.Account", b =>
                {
                    b.HasOne("MyWallet.Models.PaymentProvider", "PaymentProvider")
                        .WithMany("Accounts")
                        .HasForeignKey("PaymentProviderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Account_PaymentProvider");

                    b.HasOne("MyWallet.Models.Users", "User")
                        .WithMany("Accounts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Account_User");

                    b.Navigation("PaymentProvider");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MyWallet.Models.Payments", b =>
                {
                    b.HasOne("MyWallet.Models.Account", "Account")
                        .WithMany("Payments")
                        .HasForeignKey("PaymentProviderAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Payment_PaymentProvider");

                    b.HasOne("MyWallet.Models.PixKeys", "PixKeys")
                        .WithMany("Payments")
                        .HasForeignKey("PixKeyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Payment_PixKey");

                    b.HasOne("MyWallet.Models.Users", null)
                        .WithMany("Payments")
                        .HasForeignKey("UsersId");

                    b.Navigation("Account");

                    b.Navigation("PixKeys");
                });

            modelBuilder.Entity("MyWallet.Models.PixKeys", b =>
                {
                    b.HasOne("MyWallet.Models.Account", "Account")
                        .WithMany("PixKeys")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_PixKeys_Account");

                    b.HasOne("MyWallet.Models.PaymentProvider", "PaymentProvider")
                        .WithMany("PixKeys")
                        .HasForeignKey("PaymentProviderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("PaymentProvider");
                });

            modelBuilder.Entity("MyWallet.Models.Account", b =>
                {
                    b.Navigation("Payments");

                    b.Navigation("PixKeys");
                });

            modelBuilder.Entity("MyWallet.Models.PaymentProvider", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("PixKeys");
                });

            modelBuilder.Entity("MyWallet.Models.PixKeys", b =>
                {
                    b.Navigation("Payments");
                });

            modelBuilder.Entity("MyWallet.Models.Users", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("Payments");
                });
#pragma warning restore 612, 618
        }
    }
}
