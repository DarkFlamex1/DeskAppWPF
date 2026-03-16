using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using DeskAppWPF.Services;
using DeskAppWPF.Models;

namespace DeskAppWPF.ViewModels
{
    /// <summary>
    /// This is the view model for the calendar screen. It is responsible for exposing a 5-day calendar view with events for each day.
    /// The goal is to keep this as simple as possible, and only show time + event name for each event. 
    /// 
    /// TODO: Expand this with the ability to click to disable an event so we do not raise the desk for that event.
    /// </summary>
    internal partial class CalendarDayViewModel : ObservableObject
    {
        public DateTime Date { get; set; }
        
        // Expose a collection of strings or simple objects representing hours 0-23
        public ObservableCollection<string> Hours { get; set; } = new();

        // The events for this specific day
        public ObservableCollection<CalendarEventViewModel> DayEvents { get; set; } = new();
    }

    internal partial class CalendarViewModel : ObservableObject
    {
        private readonly ICalendarService _calendarService;
        private readonly ISettingsService _settingsService;

        [ObservableProperty]
        private ObservableCollection<CalendarEventViewModel> _events;

        [ObservableProperty]
        private ObservableCollection<CalendarDayViewModel> _days;

        public CalendarViewModel(ICalendarService calendarService, ISettingsService settingsService)
        {
            _calendarService = calendarService;
            _settingsService = settingsService;
            Events = new ObservableCollection<CalendarEventViewModel>();
            Days = new ObservableCollection<CalendarDayViewModel>();

            _ = LoadEventsAsync();
        }

        private async Task LoadEventsAsync()
        {
            var settings = _settingsService.Load();
            var icsUrl = settings?.IcsLink;

            if (string.IsNullOrEmpty(icsUrl))
            {
                return;
            }

            var cachedEvents = _calendarService.GetCachedEvents();
            if (cachedEvents != null)
            {
                UpdateEventsUI(cachedEvents);
            }

            var upcomingEvents = await _calendarService.GetUpcomingEventsAsync(icsUrl);
            UpdateEventsUI(upcomingEvents);
        }

        private void UpdateEventsUI(IEnumerable<UpcomingEvent> upcomingEvents)
        {
            var eventVms = upcomingEvents.Select(e => new CalendarEventViewModel(e, _settingsService)).ToList();

            // run this on the WPF thread sync to prevent us from r/w Events and Days when they are being rendered
            App.Current.Dispatcher.Invoke(() =>
            {
                Events.Clear();
                foreach (var evt in eventVms)
                {
                    Events.Add(evt);
                }

                Days.Clear();
                var today = DateTime.Today;
                
                // Construct the next 5 days
                for (int i = 0; i < 5; i++)
                {
                    var targetDate = today.AddDays(i);
                    var dayVm = new CalendarDayViewModel { Date = targetDate };
                    
                    // Add 24 hours for the lines
                    for (int h = 0; h < 24; h++)
                    {
                        var timeString = new DateTime(targetDate.Year, targetDate.Month, targetDate.Day, h, 0, 0).ToString("h tt");
                        dayVm.Hours.Add(timeString);
                    }

                    // Assign events to that day matching the start date
                    var todaysEvents = eventVms.Where(e => e.StartTime.Date == targetDate.Date).ToList();
                    foreach (var e in todaysEvents)
                    {
                        dayVm.DayEvents.Add(e);
                    }

                    Days.Add(dayVm);
                }
            });
        }
    }
}
