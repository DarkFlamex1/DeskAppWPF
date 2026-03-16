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
                    await PollAndApplyEventsAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in automation loop: {ex}");
                }

                var settings = _settingsService.Load();
                var pollingInterval = settings.PollingIntervalSeconds > 0 ? settings.PollingIntervalSeconds : 60;
                await Task.Delay(TimeSpan.FromSeconds(pollingInterval), stoppingToken);
            }
        }

        private async Task PollAndApplyEventsAsync()
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

            // we may only want to trigger in the buffer zone and not while the event is ongoing - TODO: Figure out if we ever want to trigger when it's ongoing. (Just assume we don't ever miss buffer zone).
            var upcomingEvent = events.FirstOrDefault(e => e.StartTime <= bufferTime && e.EndTime > now);

            if (upcomingEvent != null && upcomingEvent.Uid != _lastProcessedEventUid)
            {
                // prevent multiple triggers for the same event by checking the UID
                _lastProcessedEventUid = upcomingEvent.Uid;

                // Apply the standing preset for this event as we are now before/within the buffer zone (x minutes before event start)
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
