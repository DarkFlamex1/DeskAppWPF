using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeskAppWPF.Models;
using H.NotifyIcon.Core;
using System;

namespace DeskAppWPF.ViewModels
{
    public partial class CalendarEventViewModel : ObservableObject
    {
        private readonly UpcomingEvent _upcomingEvent;

        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private DateTime _startTime;

        [ObservableProperty]
        private DateTime _endTime;

        [ObservableProperty]
        private bool _isEnabled;

        public CalendarEventViewModel(UpcomingEvent upcomingEvent)
        {
            _upcomingEvent = upcomingEvent;
            Title = upcomingEvent.Summary;
            StartTime = upcomingEvent.StartTime;
            EndTime = upcomingEvent.EndTime;
            IsEnabled = true;
        }

        [RelayCommand]
        private void ToggleChecked()
        {
            // this is not used currently. But when a property changes especially the _isEnabled -> we don't need this command probably?
        }
    }
}
