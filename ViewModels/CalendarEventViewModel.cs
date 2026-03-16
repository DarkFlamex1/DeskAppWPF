using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeskAppWPF.Models;
using DeskAppWPF.Services;
using H.NotifyIcon.Core;
using System;

namespace DeskAppWPF.ViewModels
{
    public partial class CalendarEventViewModel : ObservableObject
    {
        private readonly UpcomingEvent _upcomingEvent;
        private readonly ISettingsService _settingsService;

        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private DateTime _startTime;

        [ObservableProperty]
        private DateTime _endTime;

        [ObservableProperty]
        private bool _isEnabled;

        public CalendarEventViewModel(UpcomingEvent upcomingEvent, ISettingsService settingsService)
        {
            _upcomingEvent = upcomingEvent;
            _settingsService = settingsService;
            Title = upcomingEvent.Summary;
            StartTime = upcomingEvent.StartTime;
            EndTime = upcomingEvent.EndTime;
            
            var settings = _settingsService.Load();
            IsEnabled = !settings.IgnoredEventUids.Contains(upcomingEvent.Uid);
        }

        // hooks into the observable property IsChanged
        partial void OnIsEnabledChanged(bool value)
        {
            var settings = _settingsService.Load();
            if (value)
            {
                if (settings.IgnoredEventUids.Contains(_upcomingEvent.Uid))
                {
                    settings.IgnoredEventUids.Remove(_upcomingEvent.Uid);
                    _settingsService.Save(settings);
                }
            }
            else
            {
                if (!settings.IgnoredEventUids.Contains(_upcomingEvent.Uid))
                {
                    settings.IgnoredEventUids.Add(_upcomingEvent.Uid);
                    _settingsService.Save(settings);
                }
            }
        }

        [RelayCommand]
        private void ToggleChecked()
        {
            // this is not used currently. But when a property changes especially the _isEnabled -> we don't need this command probably?
        }
    }
}
