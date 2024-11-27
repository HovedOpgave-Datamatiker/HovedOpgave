using System.ComponentModel;

namespace Hovedopgave.Models
{
    public class Station
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DisplayName("X Location")]
        public double LocationX { get; set; }
        [DisplayName("Y Location")]
        public double LocationY { get; set; }
        public string Notes { get; set; }
    }
}
