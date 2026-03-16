using System.Configuration;
using System.Data;
using System.Windows;
using DeskAppWPF.Services;
using DeskAppWPF.ViewModels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace DeskAppWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<IDeskService, MockDeskService>();
                    services.AddSingleton<ISettingsService, SettingsService>();
                    services.AddSingleton<ICalendarService, CalendarService>();
                    services.AddHostedService<DeskAutomationService>();
                    
                    // ViewModels
                    services.AddTransient<MainWindowViewModel>();
                    services.AddSingleton<ConnectionViewModel>();
                    
                    // Views
                    services.AddTransient<MainWindow>(provider => new MainWindow
                    {
                        DataContext = provider.GetRequiredService<MainWindowViewModel>()
                    });
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
 
            await _host.StartAsync();

            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();
            base.OnExit(e);
        }
    }
}
