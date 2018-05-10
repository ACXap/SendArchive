using System;

namespace SendArchive.Email
{
    /// <summary>
    /// Class foo message storage
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Subject message
        /// </summary>
        public string Subject { get; set; }
       
        /// <summary>
        /// Full text of the message (Text and signature)
        /// </summary>
        public string Body { get; set; }
        
        /// <summary>
        /// Array of recipient addresses
        /// </summary>
        public string[] Recipients { get; set; }
        
        /// <summary>
        /// Array file of attachments
        /// </summary>
        public string[] Attachments { get; set; }
       
        /// <summary>
        /// Is message send
        /// </summary>
        public bool IsMessageSend { get; set; }

        /// <summary>
        /// Enumerations status send message
        /// </summary>
        public StatusMessage StatusMessage { get; set; }

        /// <summary>
        /// Date send message
        /// </summary>
        public DateTime DateSend { get; set; }
       
        /// <summary>
        /// Why not send message if IsMessageSend is false
        /// </summary>
        public string WhyNotSend { get; set; }
    }
}