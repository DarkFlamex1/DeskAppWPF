using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeskAppWPF.Models;
using DeskAppWPF.Services;
using CommunityToolkit.Mvvm.Messaging;
using DeskAppWPF.Messages;

namespace DeskAppWPF.ViewModels
{
    /// <summary>
    /// Simple settings view model to expose the settings for the application.
    /// This is where we should expose ics link, 
    /// desk ip, and the buffer zone of minutes before an event to raise the desk.
    /// </summary>
    internal partial class SettingsViewModel : ObservableObject
    {
        private readonly ISettingsService _settingsService;

        [ObservableProperty]
        private string _icsLink = string.Empty;

        [ObservableProperty]
        private string _deskIpAddress = string.Empty;

        [ObservableProperty]
        private int _bufferZoneMinutes = 15;

        [ObservableProperty]
        private bool _runWhenPcLocked = false;

        [ObservableProperty]
        private int _standingPreset = 1;

        [ObservableProperty]
        private int _pollingIntervalSeconds = 60;

        public SettingsViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;
            LoadSettings();
        }

        private void LoadSettings()
        {
            var settings = _settingsService.Load();
            IcsLink = settings.IcsLink;
            DeskIpAddress = settings.DeskIpAddress;
            BufferZoneMinutes = settings.BufferZoneMinutes;
            RunWhenPcLocked = settings.RunWhenPcLocked;
            StandingPreset = settings.StandingPreset;
            PollingIntervalSeconds = settings.PollingIntervalSeconds;
        }

        [RelayCommand]
        public void Save()
        {
            var settings = new AppSettings
            {
                IcsLink = this.IcsLink,
                DeskIpAddress = this.DeskIpAddress,
                BufferZoneMinutes = this.BufferZoneMinutes,
                RunWhenPcLocked = this.RunWhenPcLocked,
                StandingPreset = this.StandingPreset,
                PollingIntervalSeconds = this.PollingIntervalSeconds
            };
            
            _settingsService.Save(settings);
            // In a more complex app, we might fire an event here to notify other view models
            // that settings have changed, or the services themselves would read from the settings.
        }

        [RelayCommand]
        public void NavigateBack()
        {
            WeakReferenceMessenger.Default.Send(new NavigationMessage("DeskControl"));
        }
    }
}
