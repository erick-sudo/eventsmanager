using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using ThAmCo.Events.Models;

namespace ThAmCo.Events.Data
{
    public class EventsDbContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<GuestBooking> GuestBookings { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Staffing> Staffing { get; set; }

        private readonly IHostEnvironment _hostEnv;
        private readonly string DbPath;

        public EventsDbContext(DbContextOptions options, IHostEnvironment env) : base(options)
        {
            _hostEnv = env;

            var folder = Environment.SpecialFolder.MyDocuments;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "ThAmCo.Events.db");

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .HasKey(k => k.EventId);

            modelBuilder.Entity<GuestBooking>()
                .HasKey(k => k.GuestBookingId);

            modelBuilder.Entity<Staff>()
                .HasKey(k => k.StaffId);

            modelBuilder.Entity<Staffing>()
                .HasKey(k => k.StaffId);
        }
    }
}
