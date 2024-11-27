using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}
