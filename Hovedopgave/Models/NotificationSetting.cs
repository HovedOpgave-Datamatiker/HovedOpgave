using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hovedopgave.Models
{
    public enum NotificationFrequency
    {
        Always,
        OnceADay,
        OnceAWeek
    }

    public class NotificationSetting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Foreign key to User
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [DisplayName("Modtag Email Notifikationer")]
        public bool EmailNotificationsEnabled { get; set; }

        [DisplayName("Hvor ofte")]
        public NotificationFrequency Frequency { get; set; }
    }
}
