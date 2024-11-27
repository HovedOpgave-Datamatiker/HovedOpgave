using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Hovedopgave.Models
{
    public class Ticket
    {

        public int Id { get; set; }
        public string Description { get; set; }
        [DisplayName("Ticket Status")]
        public bool IsFinished { get; set; }
        public DateTime Created { get; set; }
        [DisplayName("Last Update")]
        public DateTime LastUpdated { get; set; }
        [DisplayName("Ticket Priority")]
        public int Priority { get; set; }

        public Ticket()
        {
            IsFinished = false;
            Created = DateTime.Now;
            LastUpdated = DateTime.Now;
        }


    }
}
