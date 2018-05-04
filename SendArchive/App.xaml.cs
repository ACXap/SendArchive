using SendArchive.Settings;
using System.Windows;

namespace SendArchive
{
    public partial class App : Application
    {
        private readonly ISettingsService _settingsService;

        private App()
        {
            _settingsService = new SettingsService();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var vm = new MainWindowViewModel(_settingsService);
            MainWindow = new MainWindow
            {
                DataContext = vm
            };
            MainWindow.Show();
        }
    }
}