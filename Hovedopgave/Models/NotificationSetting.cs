using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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

        [ForeignKey("User")]
        public int UserId { get; set; }

        [BindNever]
        public User? User { get; set; }

        [DisplayName("Modtag Email Notifikationer")]
        public bool EmailNotificationsEnabled { get; set; }

        [DisplayName("Hvor ofte")]
        public NotificationFrequency Frequency { get; set; }
    }
}