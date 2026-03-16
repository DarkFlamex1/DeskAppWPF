using CommunityToolkit.Mvvm.ComponentModel;
using DeskAppWPF.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using DeskAppWPF.Messages;
using CommunityToolkit.Mvvm.Input;

namespace DeskAppWPF.ViewModels
{
    /// <summary>
    /// This is the main view model for the application. It is basically the shell that contains other screens that are composed with view models.
    /// This is responsible only for the navigation and startup logic of the app.
    /// </summary>
    internal partial class MainWindowViewModel : ObservableObject
    {
        // required services expected from app.xaml.cs
        private readonly IDeskService _deskService;
        private readonly ISettingsService _settingsService;

        [ObservableProperty]
        private ObservableObject _currentPage;

        public MainWindowViewModel(IDeskService deskService, ISettingsService settingsService, ICalendarService calendarService)
        {
            _deskService = deskService;
            _settingsService = settingsService;

            _currentPage = new DeskControlViewModel(_deskService, calendarService, settingsService);

            WeakReferenceMessenger.Default.Register<NavigationMessage>(this, (recipient, message) =>
            {
                if (message.Value == "Settings")
                {
                    CurrentPage = new SettingsViewModel(_settingsService);
                }
                else if (message.Value == "DeskControl")
                {
                    CurrentPage = new DeskControlViewModel(_deskService, calendarService, settingsService);
                }
            });
        }
    }
}
