using System;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace BusinessLogic
{
    public class Email
    {
        
        private readonly IConfiguration _configuration;

        // Make the constructor public
        public Email( IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public void SendMail(string subject, string body, string toEmail, string toName)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Ukukhula", _configuration["EmailConfig:Username"]));
            message.To.Add(new MailboxAddress(toName, toEmail));
            message.Subject = subject;

            // Set the body of the email
            message.Body = new TextPart("plain")
            {
                Text = body
            };
            Console.WriteLine(message);
            Console.WriteLine(_configuration["EmailConfig:Username"]);

            // Connect to the SMTP server and send the email
            using (var client = new SmtpClient())
            {
                client.Connect(_configuration["EmailConfig:Host"], 465, true);

                // Authenticate if required
                client.Authenticate(_configuration["EmailConfig:Username"], _configuration["EmailConfig:Password"]);

                // Send the email
                string response = client.Send(message);
                Console.WriteLine(response);
                // Disconnect from the server
                client.Disconnect(true);
            }
        }
    }
}
