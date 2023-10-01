using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PayMasta.Utilities.EmailUtils
{
    public class EmailUtils : IEmailUtils
    {
        //string SMTP_USERNAME = "AKIA3E2NWVBKQ7G4CPGD";
        //string SMTP_PASSWORD = "BKlkjTcRzAn4TM1qKX8p+MbLFQHjxaDnhjijo8miLqSq";
        //string CONFIGSET = "ConfigSet";
        //string HOST = "email-smtp.us-east-1.amazonaws.com";
        //string FROM = "manoj.kumar@appventurez.com";
        //string FROMNAME = "Dukkan";
        //int PORT = 587;

        public bool SendEmail(EmailModel model)
        {
            bool result = false;
            try
            {
                int port = Convert.ToInt16(AppSetting.PORT);
                MailMessage message = new MailMessage();
                message.IsBodyHtml = true;
                message.From = new MailAddress(AppSetting.FROM, AppSetting.FROMNAME);
                message.To.Add(new MailAddress(model.TO));
                message.Subject = model.Subject;
                message.Body = model.Body;
                // Comment or delete the next line if you are not using a configuration set
                // message.Headers.Add("X-SES-CONFIGURATION-SET", CONFIGSET);

                using (var client = new SmtpClient(AppSetting.HOST, port))
                {
                    // Pass SMTP credentials
                    client.Credentials =
                        new NetworkCredential(AppSetting.SMTP_USERNAME, AppSetting.SMTP_PASSWORD);

                    // Enable SSL encryption
                    client.EnableSsl = true;

                    // Try to send the message. Show status in console.

                    if (!string.IsNullOrWhiteSpace(model.AttachmentName))
                    {
                        //  MemoryStream stream = new MemoryStream(pdf);
                        System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Application.Pdf);
                        System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(model.Stream, model.AttachmentName);
                        message.Attachments.Add(attachment);
                    }

                    client.Send(message);
                    result = true;
                    WriteTextToFile("Test");

                }
            }
            catch (Exception ex)
            {
                WriteTextToFile(ex.Message);
                result = false;
            }
            return result;
        }

        public string ReadEmailformats(string Filename)
        {
            StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/HtmlTemplates/" + Filename + ""));
            string readFile = reader.ReadToEnd();
            string strEmailBody = "";
            strEmailBody = readFile;
            // strEmailBody = strEmailBody.Replace("$$user$$", username);
            return strEmailBody.ToString();
        }

        public void WriteTextToFile(string message)
        {
            try
            {
                string filePath = (HttpContext.Current.Server.MapPath("~/Logs/EmailLogs.txt"));
                using (var stream = File.Open(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    byte[] newline = Encoding.ASCII.GetBytes(Environment.NewLine);
                    stream.Write(newline, 0, newline.Length);
                    Byte[] info = new UTF8Encoding(true).GetBytes(message);
                    stream.Write(info, 0, info.Length);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                //ex.Message.ErrorLog(ex.Message, "SendEmails.cs", "WriteTextToFile");
            }
        }

        /// <summary>
        /// SendAccountStatusEmail
        /// </summary>
        /// <param name="email"></param>
        /// <param name="userName"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool SendAccountStatusEmail(string email, string userName, int status)
        {
            string filename = AppSetting.AccountStatusTemplate;
            var body = ReadEmailformats(filename);
            body = body.Replace("$$UserName$$", userName);
            body = body.Replace("$$Status$$", status == 1 ? "Blocked" : status == 2 ? "Unblocked" : status == 3 ? "Unblocked" : status == 4 ? "Verified" : "Rejected");
            var emaildata = new EmailModel
            {
                TO = email,
                Subject = "Account Status.",
                Body = body
            };
            return SendEmail(emaildata);
        }

        /// <summary>
        /// SendRegistrationEmail
        /// </summary>
        /// <param name="email"></param>
        /// <param name="userName"></param>
        /// <param name="status"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool SendRegistrationEmail(string email, string password, string userName)
        {
            string filename = AppSetting.RegistrationTemplate;
            var body = ReadEmailformats(filename);
            body = body.Replace("$$UserName$$", userName);
            body = body.Replace("$$LoginId$$", email);
            body = body.Replace("$$TempPassword$$", password);
            var emaildata = new EmailModel
            {
                TO = email,
                Subject = "PayMasta Registration",
                Body = body
            };
            return SendEmail(emaildata);
        }


        public async Task<bool> SendEmailBySendGrid()
        {

            var apiKey = AppSetting.sendgridKey;
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(AppSetting.SendEmailFrom, AppSetting.DisplayName),
                Subject = "Sending with Twilio SendGrid is Fun",
                PlainTextContent = "and easy to do anywhere, especially with C#"
            };
            msg.AddTo(new EmailAddress("rajdeep11@yopmail.com"));
            var response = await client.SendEmailAsync(msg);

            return true;
        }
    }
}
