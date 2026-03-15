using System.Configuration;
using System.Data;
using System.Windows;
using DeskAppWPF.Services;
using DeskAppWPF.ViewModels;

namespace DeskAppWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
 
            // ── 1. Build services ─────────────────────────────────────────────────
            //
            // Swap MockDeskService ↔ DeskService by changing one line here.
            // Nothing else in the codebase needs to change.
            //
            IDeskService deskService = new MockDeskService();
    
            // Sprint 2: replace with real services:
            // IDeskService deskService = new DeskService(baseUrl: "http://192.168.1.x");
            // ICalendarService calendarService = new CalendarService(icsUrl: "...");
    
            // ── 1.5 Build the Settings Service ─────────────────────────────────────
            ISettingsService settingsService = new SettingsService();

            // ── 2. Build the shell ViewModel ─────────────────────────────────────
            var mainVm = new MainWindowViewModel(deskService, settingsService);
    
            // ── 3. Build and show the window ─────────────────────────────────────
            var window = new MainWindow
            {
                DataContext = mainVm
            };
            window.Show();

            // ── 4. Run startup logic (picks first page) ──────────────────────────
            //await mainVm.InitializeAsync();
            await Task.Delay(1);
        }
    }

}
