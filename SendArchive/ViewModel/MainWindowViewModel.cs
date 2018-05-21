using SendArchive.Email;
using SendArchive.Files;
using SendArchive.Settings;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SendArchive
{
    /// <summary> 
    /// Class ViewModel for MainWindow
    /// </summary>
    public class MainWindowViewModel : BaseViewModel
    {
        #region Private Field

        // Service for working with settings
        private ISettingsService _settingsService;

        // Service for working with files
        private IFileService _fileService;

        // Service for working with email
        private IEmailService _emailService;

        // Field collection files
        private ObservableCollection<FileSpecification> _collectionFiles;

        // Field collection messages
        private ObservableCollection<ResultSending> _collectionResultSending;

        // Field total size files
        private double _totalSize = 0;

        // Field tab selected
        private TabMailWindow _tabMailWindow;

        // Field addressee
        private string _addresseeMessage;

        // Field subject
        private string _subjectMessage;

        // Field body message
        private string _textMessage;

        // Field signature message
        private string _signatureMessage;

        // Field is start sending
        private bool _isStartSending;

        // Field count sending email
        private int _countSendingEmail;

        #endregion Private Field

        #region Public Properties

        // Collection files
        public ObservableCollection<FileSpecification> CollectionFiles
        {
            get => _collectionFiles;
            set
            {
                if (_collectionFiles != value)
                {
                    _collectionFiles = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // Collection message
        public ObservableCollection<ResultSending> CollectionResultSending
        {
            get => _collectionResultSending;
            set
            {
                if (_collectionResultSending != value)
                {
                    _collectionResultSending = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // Total size files
        public double TotalSize
        {
            get => _totalSize;
            set
            {
                if (_totalSize != value)
                {
                    _totalSize = (value / 1024) / 1024;
                    NotifyPropertyChanged();
                }
            }
        }

        // Tab selected
        public TabMailWindow TabMailWindow
        {
            get => _tabMailWindow;
            set
            {
                if (_tabMailWindow != value)
                {
                    _tabMailWindow = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // Addressee
        public string AddresseeMessage
        {
            get => _addresseeMessage;
            set
            {
                if (_addresseeMessage != value)
                {
                    _addresseeMessage = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // Subject
        public string SubjectMessage
        {
            get => _subjectMessage;
            set
            {
                if (_subjectMessage != value)
                {
                    _subjectMessage = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // Body message
        public string TextMessage
        {
            get => _textMessage;
            set
            {
                if (_textMessage != value)
                {
                    _textMessage = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // Signature message
        public string SignatureMessage
        {
            get => _signatureMessage;
            set
            {
                if (_signatureMessage != value)
                {
                    _signatureMessage = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // Start sending
        public bool IsStartSending
        {
            get => _isStartSending;
            set
            {
                if (_isStartSending != value)
                {
                    _isStartSending = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // Count sending email
        public int CountSendingEmail
        {
            get => _countSendingEmail;
            set
            {
                if (_countSendingEmail != value)
                {
                    _countSendingEmail = value;
                    NotifyPropertyChanged();
                }
            }
        }

        #endregion Public Properties

        #region Command

        // Command for add files
        private RelayCommand _commandAddFiles;
        public RelayCommand CommandAddFiles
        {
            get
            {
                return _commandAddFiles ?? (_commandAddFiles = new RelayCommand(o =>
                {
                    _fileService.GetFiles(listFile =>
                    {
                        if (CollectionFiles == null)
                        {
                            CollectionFiles = new ObservableCollection<FileSpecification>(listFile);
                        }

                        //TODO You need to optimize the addition of files if the collection is not empty
                        else
                        {
                            foreach (var file in listFile)
                            {
                                CollectionFiles.Add(file);
                            }
                        }
                        TotalSize = _collectionFiles.Sum(p => p.Size);
                    });
                }));
            }
        }

        // Command for clear collection files
        private RelayCommand _commandClearCollectionFiles;
        public RelayCommand CommandClearCollectionFiles
        {
            get
            {
                return _commandClearCollectionFiles ?? (_commandClearCollectionFiles = new RelayCommand(o =>
                 {
                     TotalSize = 0;
                     CollectionFiles.Clear();
                     CollectionFiles = null;
                 }, o => CollectionFiles != null && CollectionFiles.Count != 0));
            }
        }

        // Command remove file from collection
        private RelayCommand _commandDelFile;
        public RelayCommand CommandDelFile
        {
            get
            {
                return _commandDelFile ?? (_commandDelFile = new RelayCommand(o =>
                {
                    if (o is FileSpecification)
                    {
                        _collectionFiles.Remove(o as FileSpecification);
                        TotalSize = _collectionFiles.Sum(p => p.Size);
                    }
                }));
            }
        }

        // Command open folder file
        private RelayCommand _commandOpenRepositoryFile;
        public RelayCommand CommandOpenRepositoryFile
        {
            get
            {
                return _commandOpenRepositoryFile ?? (_commandOpenRepositoryFile = new RelayCommand(o =>
                  {
                      _fileService.OpenRepositoryFile(o as string);
                  }));
            }
        }

        // Command send message
        private RelayCommand _commandSendMessage;
        public RelayCommand CommandSendMessage
        {
            get
            {
                return _commandSendMessage ?? (_commandSendMessage = new RelayCommand(o =>
                {
                    // Start sending
                    IsStartSending = true;

                    // Open tab with result;
                    TabMailWindow = TabMailWindow.TabItemResult;

                    // Creating a collection of messages with results of sending
                    CreatesMessages();

                    // Send message
                    Send();

                }, o => CollectionFiles != null && CollectionFiles.Count != 0 && !string.IsNullOrEmpty(_addresseeMessage) && !_isStartSending));
            }
        }

        // Command repeat send message
        private RelayCommand _commandRepeatSendMessage;
        public RelayCommand CommandRepeatSendMessage
        {
            get
            {
                return _commandRepeatSendMessage ?? (_commandRepeatSendMessage = new RelayCommand(async o =>
                {
                    //TODO You need to track the changes in the message, the recipients, the subject, the text

                    IsStartSending = true;

                    var result = (ResultSending)o;
                    await SendMessageAsync(result);

                    IsStartSending = false;
                }));

            }
        }

        // Command for clear coolection message
        private RelayCommand _commandClearCollectionMessage;
        public RelayCommand CommandClearCollectionMessage
        {
            get
            {
                return _commandClearCollectionMessage ?? (_commandClearCollectionMessage = new RelayCommand(o =>
                {
                    _collectionResultSending.Clear();
                    _collectionResultSending = null;
                }, o => _collectionResultSending != null && _collectionResultSending.Count > 0));
            }
        }

        // Command for cancel sending email
        private RelayCommand _commandCancelSending;
        public RelayCommand CommandCancelSending
        {
            get
            {
                return _commandCancelSending ?? (_commandCancelSending = new RelayCommand(o =>
                {
                    IsStartSending = false;
                }, o => _isStartSending));
            }
        }

        #endregion Command

        #region Public Constructor

        public MainWindowViewModel(ISettingsService settingsService, IFileService fileService, IEmailService emailService)
        {
            _settingsService = settingsService;
            _fileService = fileService;
            _emailService = emailService;
        }

        #endregion Public Constrctor

        #region Private Method

        // Method for send messages
        private async void Send()
        {
            CountSendingEmail = 0;
            foreach (var result in _collectionResultSending)
            {
                CountSendingEmail++;
                await SendMessageAsync(result);
                if (_isStartSending)
                {
                    await Task.Delay(5000); // TODO Delay in sending letters from settings
                }
            }

            // Stop sending
            IsStartSending = false;
        }

        // Method for send message
        private async Task SendMessageAsync(ResultSending resultSending)
        {
            if (_isStartSending)
            {
                resultSending.StatusMessage = StatusMessage.Sending;
                Result result = await _emailService.SendEmailAsync(new Email.Message()
                {
                    Attachments = resultSending.Message.Attachments,
                    Body = resultSending.Message.Text + Environment.NewLine + resultSending.Message.Signature,
                    Recipients = resultSending.Message.Recipients,
                    Subject = resultSending.Message.Subject,
                    CanRequestDeliveryReport = resultSending.Message.CanRequestDeliveryReport,
                    CanRequestReadReport = resultSending.Message.CanRequestReadReport
                });

                resultSending.DateTimeSending = result.DateTimeSending;

                if (result.IsSent)
                {
                    resultSending.StatusMessage = StatusMessage.Sent;
                }
                else
                {
                    resultSending.StatusMessage = StatusMessage.Fail;
                    resultSending.WhyNotSend = result.WhyNotSend;
                }
            }
            else
            {
                resultSending.StatusMessage = StatusMessage.Cancel;
                resultSending.DateTimeSending = DateTime.Now;
            }
        }

        // Method for creating a collection of messages with results of sending
        private void CreatesMessages()
        {
            // Check for files and the recipient
            if (_collectionFiles?.Count == 0 || string.IsNullOrEmpty(_addresseeMessage))
            {
                return;
            }

            // Get the recipients in the form of an array
            string[] recipients = GetRecipients(_addresseeMessage);

            if (recipients?.Length == 0)
            {
                return;
            }

            // set the initial value of the message id
            int idMessage = 0;

            // TODO You must create a topic for the language you select
            string subjectMessageNotFirstMessage = "message of";

            // TODO you need to get from the settings file
            bool canRequestDeliveryReport = true;
            bool canRequestReadReport = true;
            bool canRequestAllReportForNotFirstMessage = false;

            // Create a collection of messages to send
            CollectionResultSending = new ObservableCollection<ResultSending>();

            // Browse collection of files
            foreach (var file in _collectionFiles)
            {
                // Create new message and set ID, Recipients, Attachments
                Message msg = new Message()
                {
                    ID = idMessage,
                    Recipients = recipients,
                    Attachments = new string[] { file.Path }
                };

                // If the message is the first, then it has a subject, the text, the signature, the request report is different
                if (idMessage == 0)
                {
                    msg.Subject = $"{_subjectMessage} ({idMessage + 1} {subjectMessageNotFirstMessage} {_collectionFiles.Count})";
                    msg.Text = _textMessage;
                    msg.Signature = _signatureMessage;
                    msg.CanRequestDeliveryReport = canRequestDeliveryReport;
                    msg.CanRequestReadReport = canRequestReadReport;
                }
                else
                {
                    msg.Subject = $"{idMessage + 1} {subjectMessageNotFirstMessage} {_collectionFiles.Count}";
                    msg.Text = msg.Subject;
                    if (canRequestAllReportForNotFirstMessage)
                    {
                        msg.CanRequestDeliveryReport = msg.CanRequestReadReport = true;
                    }
                }

                // Create the result sending message
                var result = new ResultSending()
                {
                    Message = msg,
                    DateTimeSending = DateTime.Now,
                    StatusMessage = StatusMessage.ReadyToSend,
                    WhyNotSend = string.Empty
                };
                _collectionResultSending.Add(result);
                idMessage++;
            }
        }

        /// <summary>
        /// Method for converting a string of recipients to an array
        /// </summary>
        /// <param name="addresseeMessage">String recipients. Example "test@test.ru;test2@test.ru"</param>
        /// <returns></returns>
        private string[] GetRecipients(string addresseeMessage)
        {
            return addresseeMessage.Replace(" ", "").Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        }

        #endregion Private Method
    }
}