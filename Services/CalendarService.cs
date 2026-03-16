using DeskAppWPF.Models;
using Ical.Net;
using Ical.Net.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DeskAppWPF.Services
{
    internal class CalendarService : ICalendarService
    {
        private readonly HttpClient _httpClient;

        public CalendarService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<IEnumerable<UpcomingEvent>> GetUpcomingEventsAsync(string icsUrl)
        {
            if (string.IsNullOrWhiteSpace(icsUrl))
            {
                return Array.Empty<UpcomingEvent>();
            }

            try
            {
                var response = await _httpClient.GetStringAsync(icsUrl);
                var calendar = Calendar.Load(response);

                var now = CalDateTime.Now;
                var searchEnd = now.AddDays(7);
                var occurrences = calendar.GetOccurrences(now).TakeWhileBefore(searchEnd);
                
                return occurrences
                    .Where(o => o.Source is Ical.Net.CalendarComponents.CalendarEvent)
                    .Select(o => {
                        var evt = (Ical.Net.CalendarComponents.CalendarEvent)o.Source;
                        return new UpcomingEvent
                        {
                            Uid = evt.Uid,
                            Summary = evt.Summary,
                            StartTime = o.Period.StartTime?.AsUtc.ToLocalTime() ?? DateTime.MinValue,
                            EndTime = o.Period.EffectiveEndTime?.AsUtc.ToLocalTime() ?? o.Period.EndTime?.AsUtc.ToLocalTime() ?? DateTime.MaxValue
                        };
                    })
                    .OrderBy(e => e.StartTime)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching calendar: {ex.Message}");
                return Array.Empty<UpcomingEvent>();
            }
        }
    }
}
