using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using Hovedopgave.Models;

namespace Hovedopgave.Services
{
    public class NotificationMailer
    {
        private readonly SmtpClient _smtpClient;
        private readonly List<MailAddress> _recipients;
        private readonly string _fromAddress;

        public NotificationMailer(string host, int port, string fromAddress, string username, string password, bool enableSsl = true)
        {
            _smtpClient = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(username, password),
                EnableSsl = enableSsl
            };
            _recipients = new List<MailAddress>();
            _fromAddress = fromAddress;
        }

        public void AddRecipient(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                _recipients.Add(new MailAddress(email));
            }
        }

        public void ClearRecipients()
        {
            _recipients.Clear();
        }

        public void SendTicketCreatedNotification(Ticket ticket)
        {
            if (_recipients.Count == 0) return;

            var subject = $"Ny sag oprettet: #{ticket.Id}";
            var body = new StringBuilder();
            body.AppendLine("<h2>Ny Sag Oprettet</h2>");
            body.AppendLine($"<p><strong>Sag ID:</strong> {ticket.Id}</p>");
            body.AppendLine($"<p><strong>Beskrivelse:</strong> {ticket.Description}</p>");
            body.AppendLine($"<p><strong>Status:</strong> {(ticket.IsFinished ? "Lukket" : "Åben")}</p>");
            body.AppendLine($"<p><strong>Prioritet:</strong> {ticket.PriorityDescription}</p>");
            body.AppendLine("<p>Du modtager denne email i henhold til dine notifikationsindstillinger.</p>");

            SendEmail(subject, body.ToString());
        }

        public void SendTicketUpdatedNotification(Ticket ticket)
        {
            if (_recipients.Count == 0) return;

            var subject = $"Sag opdateret: #{ticket.Id}";
            var body = new StringBuilder();
            body.AppendLine("<h2>Sag Opdateret</h2>");
            body.AppendLine($"<p><strong>Sag ID:</strong> {ticket.Id}</p>");
            body.AppendLine($"<p><strong>Beskrivelse:</strong> {ticket.Description}</p>");
            body.AppendLine($"<p><strong>Status:</strong> {(ticket.IsFinished ? "Lukket" : "Åben")}</p>");
            body.AppendLine($"<p><strong>Prioritet:</strong> {ticket.PriorityDescription}</p>");
            body.AppendLine("<p>Du modtager denne email i henhold til dine notifikationsindstillinger.</p>");

            SendEmail(subject, body.ToString());
        }

        private void SendEmail(string subject, string htmlBody)
        {
            using (var message = new MailMessage())
            {
                message.From = new MailAddress(_fromAddress);
                foreach (var recipient in _recipients)
                {
                    message.To.Add(recipient);
                }
                message.Subject = subject;
                message.Body = htmlBody;
                message.IsBodyHtml = true;

                _smtpClient.Send(message);
            }
        }
    }
}
