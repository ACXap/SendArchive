﻿using System;

namespace SendArchive.Email
{
    /// <summary>
    /// Interface for description service email
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Method send message
        /// </summary>
        /// <param name="message">Messgae for send</param>
        void SendEmail(Message message);

        /// <summary>
        /// Method for create message
        /// </summary>
        /// <param name="callback">Delegate for return message</param>
        /// <param name="addressee">Arrey addressee</param>
        /// <param name="subject">Subject message</param>
        /// <param name="textMessage">Text message</param>
        /// <param name="signature">Signature message</param>
        /// <param name="attachments">Arrey path file for attachments</param>

        void CreateMessage(Action<Message> callback, string[] addressee, string subject, string textMessage, string signature, string[] attachments);
    }
}