using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace GoDaddyPleskSMTPExample
{
    public static class EmailUtil
    {
        public static string recipientEmail = "abtrack21@gmail.com,akasharve@gmail.com,akshaybangi1@gmail.com,mrunalarve001@gmail.com,chinmaybangi99@gmail.com"; // Replace with the recipient's email address
        public static string senderEmail = "track@abtrack.in"; // Replace with your sender email address
        public static string senderPassword = "abt@123456@abt"; // Replace with your sender email password
        public static string SendEmail(string subject, string body)
        {
            // GoDaddy Plesk SMTP server and port
            //string smtpServer = "smtpout.secureserver.net"; // Replace with the correct SMTP server address
            //int smtpPort = 465; // Replace with the correct SMTP port

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "relay-hosting.secureserver.net";
            smtpClient.Port = 25;
           

            smtpClient.UseDefaultCredentials = false;
           // smtpClient.EnableSsl = true;
            // Your GoDaddy Plesk email credentials
        

            // Recipient's email address
            

            try
            {
              
                    // Create the email message
                    using (MailMessage mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress(senderEmail);
                        mailMessage.To.Add(recipientEmail);
                        mailMessage.Subject = subject;
                        mailMessage.Body = body;
                        mailMessage.IsBodyHtml = true;

                        // Send the email
                        smtpClient.Timeout = 10000;
                        smtpClient.Send(mailMessage);
                        

                        return "Email sent successfully!";
                    }
                //}
            }
            catch (Exception ex)
            {
                return "Error sending email: " + ex.Message + ex.StackTrace;
            }

            //Console.ReadLine();
        }

        public static string SendEmailWithAttachment(string from, string to, string subject, string body, string attachmentFilePath)
        {
                   
            // Set up the SMTP client
            using (SmtpClient smtpClient = new SmtpClient("relay-hosting.secureserver.net"))
            {
                smtpClient.Port = 25;
    
                // Replace "your_smtp_server" with the actual SMTP server address, e.g., "smtp.example.com"

                // Set credentials if your SMTP server requires authentication
                smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                // Replace "username" and "password" with your SMTP server credentials if required.

                // Enable SSL if your SMTP server requires it
                //smtpClient.EnableSsl = true;

                // Create the email message
                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(senderEmail);
                    mailMessage.To.Add(recipientEmail);
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;

                    // Create and attach the file
                    Attachment attachment = new Attachment(attachmentFilePath, MediaTypeNames.Application.Octet);
                    ContentDisposition disposition = attachment.ContentDisposition;
                    disposition.CreationDate = File.GetCreationTime(attachmentFilePath);
                    disposition.ModificationDate = File.GetLastWriteTime(attachmentFilePath);
                    disposition.ReadDate = File.GetLastAccessTime(attachmentFilePath);

                    mailMessage.Attachments.Add(attachment);

                    // Send the email
                    smtpClient.Send(mailMessage);
                    return "Attachment Sent";
                }
            }
        }




    }
}
