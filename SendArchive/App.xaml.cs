using SendArchive.Files;
using SendArchive.Settings;
using System.Windows;
using SendArchive.Email;

namespace SendArchive
{
    public partial class App : Application
    {
        private readonly ISettingsService _settingsService;
        private readonly IFileService _fileService;
        private readonly IEmailService _emailService;

        private App()
        {
            _settingsService = new SettingsService();
            _fileService = new FileService();
            _emailService = new EmailService();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var vm = new MainWindowViewModel(_settingsService, _fileService, _emailService);
            MainWindow = new MainWindow
            {
                DataContext = vm
            };
            MainWindow.Show();
        }
    }
}