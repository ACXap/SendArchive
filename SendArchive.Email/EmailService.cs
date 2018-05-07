using System;

namespace SendArchive.Email
{
    public class EmailService : IEmailService
    {
        public void CreateMessage(Action<Message> callback, string[] addressee, string subject, string textMessage, string signature, string[] attachments)
        {
            Message message = new Message
            {
                Body = textMessage + signature,
                Subject = subject,
                Attachments = attachments,
                Addressee = addressee
            };
            callback(message);
        }


        public void SendEmail(Message message)
        {
            throw new NotImplementedException();
        }
    }
}