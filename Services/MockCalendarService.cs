using DeskAppWPF.Models;
using Ical.Net.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DeskAppWPF.Services
{
    internal class MockCalendarService : ICalendarService
    {
        public MockCalendarService()
        {
            // allow for setting up of specific settings if needed for testing
        }

        public async Task<IEnumerable<UpcomingEvent>> GetUpcomingEventsAsync(string icsUrl)
        {
            if (string.IsNullOrWhiteSpace(icsUrl))
            {
                return Array.Empty<UpcomingEvent>();
            }


            // return a mocked meeting that's always starting NOW!
            var result = new List<UpcomingEvent>();
            result.Add(new UpcomingEvent
            {
                Uid = "12345",
                Summary = "MEETING NOW: MISS IT AND YOUR FIRED",
                StartTime = DateTime.Now.ToLocalTime(),
                EndTime = DateTime.Now.AddMinutes(30).ToLocalTime()
            });

            return result.AsEnumerable();
        }
    }
}
