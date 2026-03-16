using DeskAppWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeskAppWPF.Services
{
    internal interface ICalendarService
    {
        IEnumerable<UpcomingEvent>? GetCachedEvents();
        Task<IEnumerable<UpcomingEvent>> GetUpcomingEventsAsync(string icsUrl);
    }
}
