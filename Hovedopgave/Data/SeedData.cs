using Hovedopgave.Models;
using Microsoft.EntityFrameworkCore;

namespace Hovedopgave.Data
{
    public class SeedData
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1, // Manually specify the ID since seeding requires fixed primary keys
                    Username = "admin",
                    Password = "admin123", // Ideally, passwords should be hashed
                    Role = "Admin"
                },
                new User
                {
                    Id = 2,
                    Username = "user1",
                    Password = "password123",
                    Role = "User"
                }
            );
        }
    }
}
