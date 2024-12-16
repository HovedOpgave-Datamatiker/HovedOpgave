using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hovedopgave.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        [DisplayName("Beskrivelse")]
        public string Description { get; set; }

        [DisplayName("Sagens status")]
        public bool IsFinished { get; set; }
        [DisplayName("Sag oprettet")]
        public DateTime Created { get; set; }

        [DisplayName("Sag sidst redigeret")]
        public DateTime LastUpdated { get; set; }

        [DisplayName("Sag prioritet")]
        public int Priority { get; set; }
        [DisplayName("Oprettet af")]
        public string? CreatedBy { get; set; }
        [DisplayName("Sidst redigeret af")]
        public string? LastUpdatedBy { get; set; }

        [ForeignKey("User")]
        public int? UserId { get; set; }

        [ForeignKey("Station")]
        public int? StationId { get; set; }

        public Station? Station { get; set; }

        public ICollection<User> Users { get; set; }

        public Ticket()
        {
            IsFinished = false;
            Created = DateTime.Now;
            LastUpdated = DateTime.Now;
            Users = new List<User>();
        }

        [NotMapped]
        public int[] SelectedUserIds { get; set; }

        [NotMapped]
        public string PriorityDescription
        {
            get
            {
                return Priority switch
                {
                    1 => "Lav",
                    2 => "Middel",
                    3 => "Høj",
                    _ => "Ukendt"
                };
            }
        }
    }
}
