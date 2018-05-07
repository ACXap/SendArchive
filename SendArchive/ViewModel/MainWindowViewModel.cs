using SendArchive.Files;
using SendArchive.Settings;
using SendArchive.Email;
using System.Collections.ObjectModel;
using System.Linq;
using System;

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
        private ObservableCollection<Message> _collectionMessage;

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
        public ObservableCollection<Message> CollectionMessages
        {
            get => _collectionMessage;
            set
            {
                if (_collectionMessage != value)
                {
                    _collectionMessage = value;
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
                return _commandSendMessage ?? (_commandSendMessage = new RelayCommand(async o =>
                {
                    TabMailWindow = TabMailWindow.TabItemResult;
                    CreatesMessages();
                    await SendMessagesAsync();
                }, o => CollectionFiles != null && CollectionFiles.Count != 0 && !string.IsNullOrEmpty(_addresseeMessage)));
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
                    _collectionMessage.Clear();
                    _collectionMessage = null;
                }, o => _collectionMessage != null && _collectionMessage.Count > 0));
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

        private void CreatesMessages()
        {
            if (_collectionFiles == null || _collectionFiles.Count == 0)
            {
                return;
            }

            int indexMessage = 0;
            string subjectMessageNotFirstMessage = "письмо из";
            string[] addressee = GetAddressee(_addresseeMessage);

            if (addressee == null || addressee.Length == 0)
            {
                return;
            }

            CollectionMessages = new ObservableCollection<Message>();
            foreach (var file in _collectionFiles)
            {
                if (indexMessage == 0)
                {
                    _emailService.CreateMessage(message =>
                    {
                        //_collectionMessage.Add(new Message()
                        //{
                        //    Addressee = message.Addressee,
                        //    Attachments = message.Attachments,
                        //    Body = message.Body,
                        //    Subject = message.Subject
                        //});
                        _collectionMessage.Add(message);
                    }, addressee, _subjectMessage, _textMessage, _signatureMessage, new string[] { file.Path });
                }
                else
                {
                    _emailService.CreateMessage(message =>
                    {
                        //_collectionMessage.Add(new Message()
                        //{
                        //    Addressee = message.Addressee,
                        //    Attachments = message.Attachments,
                        //    Body = message.Body,
                        //    Subject = message.Subject
                        //});
                        _collectionMessage.Add(message);
                    }, addressee, $"{indexMessage + 1} {subjectMessageNotFirstMessage} {_collectionFiles.Count}", string.Empty, string.Empty, new string[] { file.Path });
                }

                indexMessage++;
            }
        }

        private async System.Threading.Tasks.Task SendMessagesAsync()
        {
            if (_collectionMessage == null || _collectionMessage.Count == 0)
            {
                return;
            }
            foreach(var msg in _collectionMessage)
            {
                await _emailService.SendEmailAsync(msg);
            }
           
        }

        private string[] GetAddressee(string addresseeMessage)
        {
            if (string.IsNullOrEmpty(addresseeMessage))
            {
                return null;
            }
            else
            {
                return addresseeMessage.Replace(" ", "").Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            }
        }
    }
}