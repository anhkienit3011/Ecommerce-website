using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace ThucTapProject.Services
{
    public static class EmailService
    {
        public static async Task SendEmail(string ToEmail, string subject, string body)
        {
            await Console.Out.WriteLineAsync(ToEmail);
            try
            {
                var smtpClient = new SmtpClient()
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("phungdaidong1114@gmail.com", "lpgzoncuoxavbqkq"),
                    Host = "smtp.gmail.com",
                    Port = 587,
                    DeliveryMethod = SmtpDeliveryMethod.Network
            };
                await smtpClient.SendMailAsync("phungdaidong1114@gmail.com", ToEmail, subject, body);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

    }
}
