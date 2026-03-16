using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeskAppWPF.Models;
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
        private bool _isChecked = true;

        public CalendarEventViewModel(UpcomingEvent upcomingEvent)
        {
            _upcomingEvent = upcomingEvent;
            Title = upcomingEvent.Summary;
            StartTime = upcomingEvent.StartTime;
            EndTime = upcomingEvent.EndTime;
        }

        [RelayCommand]
        private void ToggleChecked()
        {
            // Future feature: toggle desk raise setting when clicked
            IsChecked = !IsChecked;
        }
    }
}
