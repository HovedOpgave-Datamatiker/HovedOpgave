using System.ComponentModel;

namespace Hovedopgave.Models
{
    public class Station
    {
        public int Id { get; set; }
        [DisplayName("Station navn")]
        public string Name { get; set; }
        [DisplayName("X Lokation")]
        public double LocationX { get; set; }
        [DisplayName("Y Lokation")]
        public double LocationY { get; set; }
        [DisplayName("Noter")]
        public string Notes { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
    }
}
