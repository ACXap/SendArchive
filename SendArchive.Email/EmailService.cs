using Microsoft.Exchange.WebServices.Data;
using System;
using System.DirectoryServices;

namespace SendArchive.Email
{
    public class EmailService : IEmailService
    {
        private ExchangeService service;

        public void CreateMessage(Action<Message> callback, string[] recipients, string subject, string textMessage, string signature, string[] attachments)
        {
            Message message = new Message
            {
                Body = textMessage + Environment.NewLine + signature,
                Subject = subject,
                Attachments = attachments,
                Recipients = recipients
            };
            callback(message);
        }

        public async System.Threading.Tasks.Task SendEmailAsync(Message message)
        {
            message.StatusMessage = StatusMessage.Sending;
            try
            {
                EmailMessage email = new EmailMessage(service);

                email.ToRecipients.AddRange(message.Recipients);
                email.Subject = message.Subject;
                email.Body = message.Body;
                email.IsDeliveryReceiptRequested = true;
                email.IsReadReceiptRequested = true;
                await email.SendAndSaveCopy();
                

                message.DateSend = DateTime.Now;
                message.StatusMessage = StatusMessage.Sent;
            }
            catch (Exception ex)
            {
                message.DateSend = DateTime.Now;
                message.StatusMessage = StatusMessage.Fail;
                message.WhyNotSend = ex.Message;
               // throw new Exception("Error");
            }
            
            
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