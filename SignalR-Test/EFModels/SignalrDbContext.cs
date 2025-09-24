using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SignalR_Test.EFModels;

public partial class SignalrDbContext : DbContext
{
    public SignalrDbContext()
    {
    }

    public SignalrDbContext(DbContextOptions<SignalrDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Connections> Connections { get; set; }

    public virtual DbSet<Persons> Persons { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("data source=DESKTOP-8MG2T45;initial catalog=SignalrDb;trusted_connection=true;trustservercertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Connections>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.SignalrId).HasMaxLength(22);
            entity.Property(e => e.Timestamp).HasColumnType("datetime");
        });

        modelBuilder.Entity<Persons>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
