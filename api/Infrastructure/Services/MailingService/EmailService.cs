using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using api.Application.Interfaces;

namespace api.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string templatePath, string verifyLink)
        {
            string emailBody = await File.ReadAllTextAsync(templatePath);
            // Replace placeholders with actual values
            emailBody = emailBody.Replace("{{VerificationLink}}", verifyLink);
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_config["EmailSettings:SenderName"], _config["EmailSettings:SenderEmail"]));
            emailMessage.To.Add(new MailboxAddress("", toEmail));
            emailMessage.Subject = subject;
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = emailBody,
                TextBody = $"Copy and paste this link into your browser: {verifyLink}"
            };

            emailMessage.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_config["EmailSettings:SmtpServer"], int.Parse(_config["EmailSettings:Port"]), true);
                await client.AuthenticateAsync(_config["EmailSettings:SenderEmail"], _config["EmailSettings:SenderPassword"]);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}