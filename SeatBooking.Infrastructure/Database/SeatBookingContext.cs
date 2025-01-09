using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SeatBooking.Domain.Common;
using SeatBooking.Domain.Entities;

namespace SeatBooking.Infrastructure.Database;

public partial class SeatBookingContext : DbContext
{
    public SeatBookingContext()
    {
    }

    public SeatBookingContext(DbContextOptions<SeatBookingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Seat> Seats { get; set; }

    public virtual DbSet<SeatColor> SeatColors { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(AppConfiguration.ConnectionString.DefaultConnection);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Bookings__3214EC07E2BDBDC4");
            entity.Property(e => e.BookingShow).HasColumnType("int");
            entity.Property(e => e.BookingTime).HasColumnType("datetime");
            entity.Property(e => e.ExpiryTime).HasColumnType("datetime");
            entity.Property(e => e.StudentName).HasMaxLength(255);
            entity.Property(e => e.IsCash).HasDefaultValue(false);
            entity.HasOne(d => d.Seat).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.SeatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bookings__SeatId__3E52440B");
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Seats__3214EC07205C06A7");

            entity.Property(e => e.Floor).HasMaxLength(50);
            entity.Property(e => e.IsBookedShowTime1).HasDefaultValue(false);
            entity.Property(e => e.IsBookedShowTime2).HasDefaultValue(false);
            entity.Property(e => e.RowChar)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.SeatColorId).HasColumnName("SeatColor_Id");

            entity.HasOne(d => d.SeatColor).WithMany(p => p.Seats)
                .HasForeignKey(d => d.SeatColorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Seats_SeatColor");
        });

        modelBuilder.Entity<SeatColor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SeatColo__3214EC0744DF3A3A");

            entity.ToTable("SeatColor");

            entity.Property(e => e.Color).HasMaxLength(20);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC0736ED7492");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Booking).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transactions_Booking");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07E11236A1");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
