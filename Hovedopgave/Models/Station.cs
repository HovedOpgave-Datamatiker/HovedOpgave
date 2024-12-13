using System.ComponentModel;

namespace Hovedopgave.Models
{
    public class Station
    {
        public int Id { get; set; }
        [DisplayName("Navn")]
        public string Name { get; set; }
        [DisplayName("X Lokation")]
        public double LocationX { get; set; }
        [DisplayName("Y Lokation")]
        public double LocationY { get; set; }
        [DisplayName("Noter")]
        public string Notes { get; set; }
    }
}
