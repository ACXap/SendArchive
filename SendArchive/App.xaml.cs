using SendArchive.Files;
using SendArchive.Settings;
using System.Windows;

namespace SendArchive
{
    public partial class App : Application
    {
        private readonly ISettingsService _settingsService;
        private readonly IFileService _fileService;

        private App()
        {
            _settingsService = new SettingsService();
            _fileService = new FileService();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var vm = new MainWindowViewModel(_settingsService, _fileService);
            MainWindow = new MainWindow
            {
                DataContext = vm
            };
            MainWindow.Show();
        }
    }
}