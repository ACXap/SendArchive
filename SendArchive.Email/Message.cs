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
        /// Request Delivery Report
        /// </summary>
        public bool CanRequestDeliveryReport { get; set; }

        /// <summary>
        /// Request reading Report
        /// </summary>
        public bool CanRequestReadReport { get; set; }

    }
}