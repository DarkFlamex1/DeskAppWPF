using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeskAppWPF.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using DeskAppWPF.Services;
using CommunityToolkit.Mvvm.Messaging;
using DeskAppWPF.Messages;
using DeskAppWPF.Models;
using System.Windows;

namespace DeskAppWPF.ViewModels
{
    /// <summary>
    /// This is the view model for the desk control screen. It is responsible for exposing presets, heights, and nav commands.
    /// This is the main/default screen of the application and the first screen that is shown on startup.
    /// </summary>
    internal partial class DeskControlViewModel : ObservableObject
    {
        // required services expected from app.xaml.cs
        private readonly IDeskService _deskService;

        [ObservableProperty]
        private float _currentHeight = 5f;

        [ObservableProperty]
        private UpcomingEvent _nextEvent;
        
        [ObservableProperty]
        private CalendarViewModel _calendarViewModel;

        [ObservableProperty]
        private ConnectionViewModel _connectionViewModel;

        public DeskControlViewModel(IDeskService deskService, ICalendarService calendarService, ISettingsService settingsService)
        {
            _deskService = deskService;
            
            CalendarViewModel = new CalendarViewModel(calendarService, settingsService);
            ConnectionViewModel = new ConnectionViewModel(deskService);

            // Fetch height...
            _ = LoadHeightAsync();
        }

        [RelayCommand]
        public async Task LoadHeightAsync()
        {
            CurrentHeight = await _deskService.GetHeightAsync();
        }

        [RelayCommand]
        public async Task SetPresetAsync(string preset)
        {
            await _deskService.SetPresetAsync(int.Parse(preset));
        }

        [RelayCommand]
        public void Navigate(string destination)
        {
            WeakReferenceMessenger.Default.Send(new NavigationMessage(destination));
        }
    }
}
