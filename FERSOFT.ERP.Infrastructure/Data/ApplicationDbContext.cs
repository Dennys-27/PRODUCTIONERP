using FERSOFT.ERP.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FERSOFT.ERP.Infrastructure.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : IdentityDbContext<AppUsuario>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuRol> MenuRoles { get; set; }
        public DbSet<BillboardEntity> Billboards { get; set; }
        public DbSet<BookingEntity> Bookings { get; set; }
        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<MovieEntity> Movies { get; set; }
        public DbSet<RoomEntity> Rooms { get; set; }
        public DbSet<SeatEntity> Seats { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // configuraciones adicionales aquí

            // Relación: Booking → Seat
            builder.Entity<BookingEntity>()
                .HasOne(b => b.Seat)
                .WithMany()
                .HasForeignKey(b => b.SeatId)
                .OnDelete(DeleteBehavior.Restrict); // Evita cascadas múltiples

            // Relación: Booking → Customer
            builder.Entity<BookingEntity>()
                .HasOne(b => b.Customer)
                .WithMany()
                .HasForeignKey(b => b.CustomerId)
                .OnDelete(DeleteBehavior.Restrict); // Evita cascadas múltiples

            // Relación: Booking → Billboard
            builder.Entity<BookingEntity>()
                .HasOne(b => b.Billboard)
                .WithMany()
                .HasForeignKey(b => b.BillboardId)
                .OnDelete(DeleteBehavior.Cascade); // Solo esta con cascada

        }
    }

}
