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
                    Id = 1,
                    Username = "admin",
                    Password = "admin123",
                    Role = "Admin",
                    FullName = "Admin Adminsen",
                    Initials = "AA"
                },
                new User
                {
                    Id = 2,
                    Username = "user1",
                    Password = "password123",
                    Role = "User",
                    FullName = "User Useren",
                    Initials = "UU"
                },
                new User
                {
                    Id = 3,
                    Username = "felt1",
                    Password = "password123",
                    Role = "Felt",
                    FullName = "Felt Feltsen",
                    Initials = "FF"
                },
                new User
                {
                    Id = 4,
                    Username = "kontor1",
                    Password = "password123",
                    Role = "Kontor",
                    FullName = "Kontor Kontorsen",
                    Initials = "KK"
                }
            );

            modelBuilder.Entity<Station>().HasData(
                new Station
                {
                    Id = 1,
                    Name = "Station 1",
                    LocationX = 1.0,
                    LocationY = 2.0,
                    Notes = "This is station 1"
                },
                new Station
                {
                    Id = 2,
                    Name = "Station 2",
                    LocationX = 3.0,
                    LocationY = 4.0,
                    Notes = "This is station 2"
                }
            );

            modelBuilder.Entity<Ticket>().HasData(
                new Ticket
                {
                    Id = 1,
                    Description = "Ticket 1",
                    Priority = 1
                }
                );


        }
    }
}
