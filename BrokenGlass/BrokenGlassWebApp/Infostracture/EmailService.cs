using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace BrokenGlassWebApp.Infostracture
{
    public class EmailService
    {
        public async Task<MailMessage> SendMessageAsync(string emailTo,string subject ,string messageBody)
        {
            if (string.IsNullOrEmpty(emailTo) && string.IsNullOrWhiteSpace(emailTo))
            {
                throw new ArgumentNullException("Email адресата был передан пустым.");
            }

            try
            {
                using (MailMessage emailMessage = new MailMessage())
                using (SmtpClient smtp = new SmtpClient())
                {
                    emailMessage.To.Add(new MailAddress(emailTo));
                    emailMessage.Subject = subject;
                    emailMessage.IsBodyHtml = true;
                    emailMessage.Body = messageBody;
                    await smtp.SendMailAsync(emailMessage);

                    return emailMessage;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}