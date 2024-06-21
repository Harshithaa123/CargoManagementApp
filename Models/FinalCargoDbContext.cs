using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Cargo_FinalApplication.Models;

public partial class FinalCargoDbContext : DbContext
{
    public FinalCargoDbContext()
    {
    }

    public FinalCargoDbContext(DbContextOptions<FinalCargoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Cargo> Cargos { get; set; }

    public virtual DbSet<GatePass> GatePasses { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserDetail> UserDetails { get; set; }

    public virtual DbSet<UserRegistrationsTable> UserRegistrationsTables { get; set; }

    public virtual DbSet<Warehouse> Warehouses { get; set; }

    public virtual DbSet<WarehouseInventory> WarehouseInventories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-9H0U0A1\\SQLEXPRESS; Initial Catalog=FinalCargoDb; Integrated Security=True; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Admin");

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
        });

        modelBuilder.Entity<Cargo>(entity =>
        {
            entity.HasKey(e => e.CargoId).HasName("PK__Cargo__B4E665CDB869BFC0");

            entity.ToTable("Cargo");

            entity.Property(e => e.CargoName).HasMaxLength(100);
            entity.Property(e => e.DateStored).HasColumnType("datetime");
            entity.Property(e => e.Weight).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.Cargos)
                .HasForeignKey(d => d.WarehouseId)
                .HasConstraintName("FK__Cargo__Warehouse__46E78A0C");
        });

        modelBuilder.Entity<GatePass>(entity =>
        {
            entity.HasKey(e => e.GatePassId).HasName("PK__GatePass__53AA30ACD6C1C976");

            entity.Property(e => e.DispatchDate).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.GatePasses)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__GatePasse__Creat__4E88ABD4");

            entity.HasOne(d => d.Order).WithMany(p => p.GatePasses)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__GatePasse__Order__4D94879B");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BCFDBED69C0");

            entity.Property(e => e.CargoName).HasMaxLength(100);
            entity.Property(e => e.IsOutgoing).HasDefaultValue(true);
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.FromWarehouse).WithMany(p => p.OrderFromWarehouses)
                .HasForeignKey(d => d.FromWarehouseId)
                .HasConstraintName("FK__Orders__FromWare__412EB0B6");

            entity.HasOne(d => d.Receiver).WithMany(p => p.OrderReceivers)
                .HasForeignKey(d => d.ReceiverId)
                .HasConstraintName("FK__Orders__Receiver__403A8C7D");

            entity.HasOne(d => d.Sender).WithMany(p => p.OrderSenders)
                .HasForeignKey(d => d.SenderId)
                .HasConstraintName("FK__Orders__SenderId__3F466844");

            entity.HasOne(d => d.ToWarehouse).WithMany(p => p.OrderToWarehouses)
                .HasForeignKey(d => d.ToWarehouseId)
                .HasConstraintName("FK__Orders__ToWareho__4222D4EF");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__Reports__D5BD48056AC7BFCD");

            entity.Property(e => e.GeneratedDate).HasColumnType("datetime");
            entity.Property(e => e.ReportName).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CB46969DE");

            entity.Property(e => e.PasswordHash).HasMaxLength(256);
            entity.Property(e => e.Role).HasMaxLength(20);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<UserDetail>(entity =>
        {
            entity.HasKey(e => e.UserDetailId).HasName("PK__UserDeta__564F56B231F3E490");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);

            entity.HasOne(d => d.User).WithMany(p => p.UserDetails)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserDetai__UserI__38996AB5");
        });

        modelBuilder.Entity<UserRegistrationsTable>(entity =>
        {
            entity.HasKey(e => e.RegistrationId).HasName("PK__UserRegi__6EF58810D2DB9CEE");

            entity.ToTable("UserRegistrationsTable");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.PasswordHash).HasMaxLength(256);
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.WarehouseId).HasName("PK__Warehous__2608AFF9530E4C24");

            entity.Property(e => e.Location).HasMaxLength(255);
            entity.Property(e => e.MobileNum).HasMaxLength(15);
            entity.Property(e => e.WarehouseName).HasMaxLength(100);
        });

        modelBuilder.Entity<WarehouseInventory>(entity =>
        {
            entity.HasKey(e => new { e.WarehouseId, e.CargoId }).HasName("PK__Warehous__ED46C9A5555A8F5E");

            entity.ToTable("WarehouseInventory");

            entity.HasOne(d => d.Cargo).WithMany(p => p.WarehouseInventories)
                .HasForeignKey(d => d.CargoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Warehouse__Cargo__4AB81AF0");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.WarehouseInventories)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Warehouse__Wareh__49C3F6B7");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
