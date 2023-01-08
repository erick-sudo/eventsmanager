using Microsoft.EntityFrameworkCore;
using ThAmCo.Catering.Models;

namespace ThAmCo.Catering.Data
{
    public class CateringDbContext : DbContext
    {
        //Database Sets
        public DbSet<FoodBooking> FoodBookings { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuFoodItem> MenuFoodItems { get; set; }

        private readonly IHostEnvironment _hostEnv;
        private readonly string DbPath;

        public CateringDbContext(DbContextOptions options, IHostEnvironment env) : base(options)
        {
            _hostEnv = env;

            var folder = Environment.SpecialFolder.MyDocuments;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "ThAmCo.Catering.db");

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Menu>()
                .HasKey(k => k.MenuId);

            modelBuilder.Entity<FoodBooking>()
                .HasKey(k => k.FoodBookingId);

            modelBuilder.Entity<MenuFoodItem>()
                .HasKey(k => k.MenuFoodItemId);
                

            modelBuilder.Entity<FoodItem>()
                .HasKey(k => k.FoodItemId);

            //Load some data
            modelBuilder.Entity<FoodBooking>()
                .HasData(
                    new FoodBooking { FoodBookingId = 1, ClientReferenceId = 101, NumberOfGuests = 78, MenuId = 100011 },
                    new FoodBooking { FoodBookingId = 2, ClientReferenceId = 100, NumberOfGuests = 67, MenuId = 100013 },
                    new FoodBooking { FoodBookingId = 3, ClientReferenceId = 108, NumberOfGuests = 65, MenuId = 100013 },
                    new FoodBooking { FoodBookingId = 4, ClientReferenceId = 118, NumberOfGuests = 23, MenuId = 100014 }
                );
            modelBuilder.Entity<Menu>()
                .HasData(
                    new Menu
                    {
                        MenuId = 100011,
                        MenuName = "Ramen Rater"
                    },
                    new Menu
                    {
                        MenuId = 100012,
                        MenuName = "Wincer"
                    },
                    new Menu
                    {
                        MenuId = 100013,
                        MenuName = "Marine"
                    },
                    new Menu
                    {
                        MenuId = 100014,
                        MenuName = "Rose Mont"
                    }
                );

            modelBuilder.Entity<MenuFoodItem>()
                .HasData(
                    new MenuFoodItem { MenuFoodItemId = 4, FoodItemId = 109, MenuId = 100011 },
                    new MenuFoodItem { MenuFoodItemId = 5, FoodItemId = 100, MenuId = 100011 },
                    new MenuFoodItem { MenuFoodItemId = 6, FoodItemId = 104, MenuId = 100011 },
                    new MenuFoodItem { MenuFoodItemId = 7, FoodItemId = 102, MenuId = 100012 },
                    new MenuFoodItem { MenuFoodItemId = 8, FoodItemId = 101, MenuId = 100012 },
                    new MenuFoodItem { MenuFoodItemId = 9, FoodItemId = 103, MenuId = 100012 },
                    new MenuFoodItem { MenuFoodItemId = 10, FoodItemId = 104, MenuId = 100012 },
                    new MenuFoodItem { MenuFoodItemId = 15, FoodItemId = 107, MenuId = 100012 },
                    new MenuFoodItem { MenuFoodItemId = 16, FoodItemId = 108, MenuId = 100013 },
                    new MenuFoodItem { MenuFoodItemId = 17, FoodItemId = 100, MenuId = 100013 },
                    new MenuFoodItem { MenuFoodItemId = 18, FoodItemId = 103, MenuId = 100013 },
                    new MenuFoodItem { MenuFoodItemId = 19, FoodItemId = 104, MenuId = 100014 },
                    new MenuFoodItem { MenuFoodItemId = 20, FoodItemId = 105, MenuId = 100014 },
                    new MenuFoodItem { MenuFoodItemId = 21, FoodItemId = 107, MenuId = 100014 }
                );

            modelBuilder.Entity<FoodItem>()
                .HasData(
                    new FoodItem { FoodItemId = 102, FoodItemName = "Whole Grains", Description = "A good source of fiber has 3 to 4 grams of fiber per serving.", UnitPrice = 45.65F },
                    new FoodItem { FoodItemId = 105, FoodItemName = "Berries", Description = "Berries such as raspberries, blueberries, blackberries and strawberries.", UnitPrice = 75.66F },
                    new FoodItem { FoodItemId = 107, FoodItemName = "Flaxseed, Nuts and Seeds", Description = "Ground flaxseed or other seeds to food each day ", UnitPrice = 75.54F },
                    new FoodItem { FoodItemId = 109, FoodItemName = "Samp", Description = "Try a traditional umngqusho samp and beans dish to suit your taste buds.", UnitPrice = 78.34F }
                );
        }
    }
}
