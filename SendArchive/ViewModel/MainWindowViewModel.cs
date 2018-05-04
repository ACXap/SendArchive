using SendArchive.Files;
using SendArchive.Settings;
using System.Collections.ObjectModel;
using System.Linq;

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

        // Service for working wih files
        private IFileService _fileService;

        // Field collection files
        private ObservableCollection<FileSpecification> _collectionFiles;

        // Field total size files
        private double _totalSize = 0;
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

        #endregion Command

        #region Public Constructor

        public MainWindowViewModel(ISettingsService settingsService, IFileService fileService)
        {
            _settingsService = settingsService;
            _fileService = fileService;
        }

        #endregion Public Constrctor
    }
}