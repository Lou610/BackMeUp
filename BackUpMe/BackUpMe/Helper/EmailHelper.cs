using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using BackUpMe.Models;

namespace BackUpMe.Helper
{
    public class EmailHelper
    {

        public void SendEmail(string fileName)
        {
            EmailSettings emailSettings = new EmailSettings();



            try
            {
                emailSettings = getCredentials();

                // Set up the SMTP client and credentials
                SmtpClient smtpClient = new SmtpClient(emailSettings.Host);
                smtpClient.Port = emailSettings.port; // Port number for the SMTP server
                smtpClient.Credentials = new NetworkCredential(emailSettings.FromAddress, emailSettings.Password);
                smtpClient.EnableSsl = emailSettings.EnableSsl;

                // Create the email message
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(emailSettings.FromAddress!);
                mailMessage.To.Add(emailSettings.ToAddress!);
                
                mailMessage.Subject = "File Uploaded" + DateTime.Now.ToString();
                mailMessage.Body = "Your file " + fileName + " has been uploaded successfully";

                // Send the email
                smtpClient.Send(mailMessage);

            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }


        public EmailSettings getCredentials()
        {
            EmailSettings emailSettings = new EmailSettings();  

            emailSettings.FromAddress = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Email")["FromAddress"];
            emailSettings.Password = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Email")["Password"];
            emailSettings.Host = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Email")["Host"];
            emailSettings.port = Convert.ToInt32(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Email")["port"]);
            emailSettings.EnableSsl = Convert.ToBoolean(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Email")["EnableSsl"]);
            emailSettings.ToAddress = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Email")["ToAddress"];

            return emailSettings;
        }

    }
}
