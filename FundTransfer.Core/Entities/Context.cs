using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundTransfer.Core.Entities
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            base.OnConfiguring(optionBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>().Property(u => u.TranAmount).HasPrecision(18,2);
            modelBuilder.Entity<Account>().Property(u => u.AccountBalance).HasPrecision(18,2);
            modelBuilder.Entity<Customer>().HasIndex(u => u.Email).IsUnique(true);
            modelBuilder.Entity<Customer>().HasIndex(u => u.PhoneNo).IsUnique(true);
            modelBuilder.Entity<Account>().HasIndex(u => u.AccountNumber).IsUnique(true);
            modelBuilder.Entity<Account>()
            .HasOne(a => a.Customer)
            .WithMany(a => a.Account)
            .HasForeignKey(c => c.CustId);
            modelBuilder.Entity<Transaction>()
            .HasOne(a => a.Account)
            .WithMany(a => a.Transaction)
            .HasForeignKey(c => c.AcctId);
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
