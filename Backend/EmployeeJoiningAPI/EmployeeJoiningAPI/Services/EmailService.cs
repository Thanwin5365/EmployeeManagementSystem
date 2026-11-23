
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace EmployeeJoiningAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(
            string toEmail,
            string subject,
            string body)
        {
            var emailAddress =
                _configuration["EmailSettings:Email"]
                ?? throw new InvalidOperationException(
                    "Email address is not configured.");

            var appPassword =
                _configuration["EmailSettings:AppPassword"]
                ?? throw new InvalidOperationException(
                    "Gmail App Password is not configured.");

            var message = new MimeMessage();

            message.From.Add(
                new MailboxAddress(
                    "Alpha Data Employee Portal",
                    emailAddress));

            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = body
            };

            using var smtp = new SmtpClient();

            using var cts = new CancellationTokenSource(
                TimeSpan.FromSeconds(15));

            try
            {
                Console.WriteLine("SMTP: Connecting...");

                await smtp.ConnectAsync(
                    "smtp.gmail.com",
                    587,
                    SecureSocketOptions.StartTls,
                    cts.Token);

                Console.WriteLine("SMTP: Connected successfully");

                await smtp.AuthenticateAsync(
                    emailAddress,
                    appPassword,
                    cts.Token);

                Console.WriteLine("SMTP: Authentication successful");

                await smtp.SendAsync(message, cts.Token);

                Console.WriteLine("SMTP: Email sent successfully");

                await smtp.DisconnectAsync(true, cts.Token);
            }
            catch (OperationCanceledException)
            {
                throw new TimeoutException(
                    "Email sending timed out after 15 seconds.");
            }
        }
    }
}
