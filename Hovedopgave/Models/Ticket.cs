using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
                    1 => "Low",
                    2 => "Medium",
                    3 => "High",
                    _ => "Unknown"
                };
            }
        }
    }
}
