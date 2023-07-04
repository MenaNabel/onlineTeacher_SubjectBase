using Microsoft.Extensions.Configuration;
using OnlineTeacher.Shared.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.Shared.Services
{
    public class SendGrideMailServices : IMailServices
    {
        private IConfiguration _configuration;

        public SendGrideMailServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string ToEmail, string Subject, string Content)
        {
            var apiKey = _configuration["SendGrideKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("test@authdemo.com", "JWT Auth Demo");
            var to = new EmailAddress(ToEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, Subject, Content, Content);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
