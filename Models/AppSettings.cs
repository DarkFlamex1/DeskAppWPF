using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeskAppWPF.Models
{
    public class AppSettings
    {
        public string IcsLink { get; set; } = string.Empty;
        public string DeskIpAddress { get; set; } = string.Empty;
        public int BufferZoneMinutes { get; set; } = 1;
        public bool RunWhenPcLocked { get; set; } = false;
        public int StandingPreset { get; set; } = 1;
        public int PollingIntervalSeconds { get; set; } = 60;
    }
}
