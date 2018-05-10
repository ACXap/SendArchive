using Microsoft.Exchange.WebServices.Data;
using System;
using System.DirectoryServices;

namespace SendArchive.Email
{
    public class EmailService : IEmailService
    {
        private ExchangeService service;


        public async System.Threading.Tasks.Task<Result> SendEmailAsync(Message message)
        {
            Result result = new Result();

            try
            {
                EmailMessage email = new EmailMessage(service);

                email.ToRecipients.AddRange(message.Recipients);
                email.Subject = message.Subject;
                foreach(var path in message.Attachments)
                {
                    email.Attachments.AddFileAttachment(path);
                }
                email.Body = message.Body;
                email.IsDeliveryReceiptRequested = message.CanRequestDeliveryReport;
                email.IsReadReceiptRequested = message.CanRequestReadReport;
                await System.Threading.Tasks.Task.Delay(4000);
                await email.SendAndSaveCopy();
                result.DateTimeSending = DateTime.Now;
                result.IsSent = true;
            }
            catch (Exception ex)
            {
                result.IsSent = false;
                result.DateTimeSending = DateTime.Now;
                result.WhyNotSend = ex.Message;
            }

            return result;
        }

        public EmailService()
        {
            Connect();
        }

        private void Connect()
        {
            service = new ExchangeService(ExchangeVersion.Exchange2007_SP1)
            {
                UseDefaultCredentials = true
            };
            string emailUser = GetEmailUser();
            service.AutodiscoverUrl(emailUser, RedirectionUrlValidationCallback);
        }

        private string GetEmailUser()
        {
            string email = string.Empty;
            using (DirectorySearcher dirSearcher = new DirectorySearcher())
            {
                using (DirectoryEntry entry = new DirectoryEntry(dirSearcher.SearchRoot.Path))
                {
                    dirSearcher.Filter = "(&(objectClass=user)(objectcategory=person)(mail=" + Environment.UserName + "*))";
                    SearchResult srEmail = dirSearcher.FindOne();
                    string propName = "mail";
                    ResultPropertyValueCollection valColl = srEmail.Properties[propName];
                    email = valColl[0].ToString();
                }
            }
            return email;
        }

        private static bool RedirectionUrlValidationCallback(string redirectionUrl)
        {
            // The default for the validation callback is to reject the URL.
            bool result = false;

            Uri redirectionUri = new Uri(redirectionUrl);

            // Validate the contents of the redirection URL. In this simple validation
            // callback, the redirection URL is considered valid if it is using HTTPS
            // to encrypt the authentication credentials. 
            if (redirectionUri.Scheme == "https")
            {
                result = true;
            }
            return result;
        }
    }
}