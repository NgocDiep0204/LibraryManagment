using api_librarymanagment.Models.ServiceEmail;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace api_librarymanagment.Services
{
    public class EmailService: IEmailService
    {
        private readonly EmailConfiguration emailConfiguration;
    
        public EmailService(EmailConfiguration emailConfiguration)
        {
            this.emailConfiguration = emailConfiguration;
        }
        public void SendEmail(Messages message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }
        private MimeMessage CreateEmailMessage(Messages messages)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", emailConfiguration.From));
            emailMessage.To.AddRange(messages.To);
            emailMessage.Subject = messages.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text)
            {
                Text = messages.Content
            };
            return emailMessage;
        }
        private void Send(MimeMessage mailMessage)
        {
            var client = new SmtpClient();
            try
            {
                client.Connect(emailConfiguration.SmtpServer, emailConfiguration.Port, SecureSocketOptions.StartTls);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(emailConfiguration.UserName, emailConfiguration.Password);
                client.Send(mailMessage);
                
            }
            catch (Exception ex)
            {
                 throw new ApplicationException("Unable to send email.", ex);
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }
    }
}
