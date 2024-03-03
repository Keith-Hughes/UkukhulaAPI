using System;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using Microsoft.Extensions.Configuration;


namespace BusinessLogic
{
    public class Email
    {
        private readonly SmtpClient _smtpClient;
        private readonly IConfiguration _configuration;
        Email(SmtpClient smtpClient ,IConfiguration configuration) { 
            this._smtpClient = smtpClient;
            this._configuration = configuration;
        }

        public void sendMail(string subject, string body, string toEmail)
        {
            //_smtpClient.Connect(_configuration["EmailConfig:Host"], 
        }
    }
}
