using SendArchive.Settings;

namespace SendArchive
{
    /// <summary> 
    /// Class ViewModel for MainWindow
    /// </summary>
    public class MainWindowViewModel : BaseViewModel
    {
        private ISettingsService _settingsService;

        public MainWindowViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }
    }
}
