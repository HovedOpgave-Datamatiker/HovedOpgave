using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Hovedopgave.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Kommentar")]
        public string Text { get; set; }

        [DisplayName("Oprettet")]
        public DateTime Created { get; set; }

        [DisplayName("Oprettet af")]
        public string? CreatedBy { get; set; }

        [ForeignKey("Ticket")]
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }

        public Comment()
        {
            Created = DateTime.Now;
        }

    }
}
