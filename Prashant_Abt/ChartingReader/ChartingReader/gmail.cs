using System;
using System.Net;
using System.Net.Mail;

namespace GmailSMTPExample
{
    public static class EmailUtil
    {
        public static  string  SendEmail (string subject, string body)
        {
            // Gmail SMTP server and port
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587;

            // Your Gmail credentials
            string senderEmail = "abtrack21@gmail.com";
            string senderPassword = "amujkfgwgyjxhsrz";

            // Recipient's email address
            string recipientEmail = "abtrack21@gmail.com";

            try
            {
                // Create and configure the SMTP client
                using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
                {
                    smtpClient.EnableSsl = true;
                    //smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

                    // Create the email message
                    using (MailMessage mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress(senderEmail);
                        mailMessage.To.Add(new MailAddress(recipientEmail));
                        mailMessage.Subject = subject;
                        mailMessage.Body = body;

                        // Send the email
                        // smtpClient.SendAsync(mailMessage, null);
                        smtpClient.Send(mailMessage);

                        Console.WriteLine("Email sent successfully!");
                        return "OK";
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error sending email: " + ex.Message;
            }

            // Console.ReadLine();
        }
    }
}
