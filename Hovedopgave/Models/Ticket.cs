namespace Hovedopgave.Models
{
    public class Ticket
    {

        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsFinished { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }
        public int Priority { get; set; }

        public Ticket()
        {
            IsFinished = false;
            Created = DateTime.Now;
            LastUpdated = DateTime.Now;
        }


    }
}
