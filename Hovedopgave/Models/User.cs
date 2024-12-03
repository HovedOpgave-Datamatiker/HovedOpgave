﻿using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hovedopgave.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Specifies auto-increment
        public int Id { get; set; }
        [DisplayName("Brugernavn")]
        public string Username { get; set; }
        [DisplayName("Kodeord")]
        public string Password { get; set; }
        [DisplayName("Rolle")]
        public string Role { get; set; }

        public ICollection<Ticket> Tickets { get; set; }

        public User()
        {
            Tickets = new List<Ticket>();
        }
    }
}
