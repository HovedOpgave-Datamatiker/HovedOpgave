using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hovedopgave.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DisplayName("Brugernavn")]
        public string Username { get; set; }
        [DisplayName("Fulde Navn")]
        public string? FullName { get; set; }
        [DisplayName("Initialer")]
        public string Initials { get; set; }
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
