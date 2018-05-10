using System;

namespace SendArchive.ViewModel.Data
{
    public class ResultSending : BaseViewModel
    {
        private StatusMessage _statusMessage;
        private DateTime _dateTimeSending;
        private string _whyNotSend;

        public Message Message { get; set; }

        public StatusMessage StatusMessage
        {
            get => _statusMessage;
            set
            {
                if (_statusMessage != value)
                {
                    _statusMessage = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public DateTime DateTimeSending
        {
            get => _dateTimeSending;
            set
            {
                if (_dateTimeSending != value)
                {
                    _dateTimeSending = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string WhyNotSend
        {
            get => _whyNotSend;
            set
            {
                if(_whyNotSend!=value)
                {
                    _whyNotSend = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}