using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Win32;

namespace DeskAppWPF.Services
{
    /// <summary>
    /// Background serviec that constantly polls the ICS link for new events and for events coming up within the buffer zone.
    /// </summary>
    internal class DeskAutomationService : BackgroundService
    {
        private readonly ISettingsService _settingsService;
        private readonly ICalendarService _calendarService;
        private readonly IDeskService _deskService;
        
        private bool _isPcLocked = false;
        private string _lastProcessedEventUid = string.Empty;

        public DeskAutomationService(
            ISettingsService settingsService, 
            ICalendarService calendarService, 
            IDeskService deskService)
        {
            _settingsService = settingsService;
            _calendarService = calendarService;
            _deskService = deskService;

            SystemEvents.SessionSwitch += OnSessionSwitch;
        }

        private void OnSessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            if (e.Reason == SessionSwitchReason.SessionLock)
            {
                _isPcLocked = true;
            }
            else if (e.Reason == SessionSwitchReason.SessionUnlock)
            {
                _isPcLocked = false;
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Initial delay to avoid immediately running at startup while UI is loading
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await PerformDeskCheckAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in automation loop: {ex}");
                }

                // Poll every 10 seconds - this can be adjusted as needed, but should be frequent enough to catch events in a timely manner without being too resource intensive.
                // TODO: Implement this as a AppSettings option with a default of 10 seconds.
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        private async Task PerformDeskCheckAsync()
        {
            var settings = _settingsService.Load();
            
            if (_isPcLocked && !settings.RunWhenPcLocked)
            {
                // Logic disabled while locked.
                return;
            }

            var events = await _calendarService.GetUpcomingEventsAsync(settings.IcsLink);

            var now = DateTime.Now;
            var bufferTime = now.AddMinutes(settings.BufferZoneMinutes);

            var upcomingEvent = events.FirstOrDefault(e => e.StartTime <= bufferTime && e.EndTime > now);

            if (upcomingEvent != null && upcomingEvent.Uid != _lastProcessedEventUid)
            {
                _lastProcessedEventUid = upcomingEvent.Uid;
                await _deskService.SetPresetAsync(settings.StandingPreset);
            }
        }

        public override void Dispose()
        {
            SystemEvents.SessionSwitch -= OnSessionSwitch;
            base.Dispose();
        }
    }
}
