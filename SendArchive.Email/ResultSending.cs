using System;

namespace SendArchive.Email
{
    public class Result
    {
        public bool IsSent { get; set; }
        public DateTime DateTimeSending { get; set; }
        public string WhyNotSend { get; set; }
    }
}