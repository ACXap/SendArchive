namespace SendArchive
{
    /// <summary> 
    /// Class Message for  storage
    /// </summary>
    public class Message
    {
        /// <summary>
        /// ID message
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Array addresses recipients
        /// </summary>
        public string[] Recipients { get; set; }

        /// <summary>
        /// Subject message
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Text message
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Signature at the end of the message text
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// Paths files attacments 
        /// </summary>
        public string[] Attacments { get; set; }

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