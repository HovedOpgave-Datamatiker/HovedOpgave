using Microsoft.EntityFrameworkCore;
using Hovedopgave.Models;

namespace Hovedopgave.Data
{
    public class HovedopgaveContext : DbContext
    {
        public HovedopgaveContext (DbContextOptions<HovedopgaveContext> options)
            : base(options)
        {
        }

        public DbSet<Hovedopgave.Models.Ticket> Ticket { get; set; } = default!;
        public DbSet<Hovedopgave.Models.User> User { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            SeedData.Seed(modelBuilder);
            modelBuilder.Entity<Ticket> ().HasMany(t => t.Users).WithMany(u => u.Tickets)
                .UsingEntity(j => j.ToTable("TicketUser"));

        }
        public DbSet<Hovedopgave.Models.Station> Station { get; set; } = default!;
    }
}