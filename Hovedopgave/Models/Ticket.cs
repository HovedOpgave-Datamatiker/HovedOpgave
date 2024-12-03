using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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

        [ForeignKey("User")]
        public int? UserId { get; set; }

        public User? User { get; set; }

        // Constructor to set default values
        public Ticket()
        {
            IsFinished = false;
            Created = DateTime.Now;
            LastUpdated = DateTime.Now;
        }

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
