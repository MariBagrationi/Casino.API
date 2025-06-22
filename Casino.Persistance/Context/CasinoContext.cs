﻿using Casino.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Casino.Persistance.Context;

public partial class CasinoContext : DbContext
{
    public CasinoContext()
    {
    }

    public CasinoContext(DbContextOptions<CasinoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn(); 

            entity.Property(e => e.Balance).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(150);
               
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.ToTable("Transaction");

            entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Date).HasColumnType("datetime");

            entity.HasOne(t => t.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.UserId);
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
