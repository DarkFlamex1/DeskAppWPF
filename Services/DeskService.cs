using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using DeskAppWPF.Models;

namespace DeskAppWPF.Services
{
    internal class DeskService : IDeskService
    {
        private readonly ISettingsService _settingsService;
        private static readonly HttpClient _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(2) };
        private bool _isConnected = false;
        public DeskService(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        // HTTP implementation of IDeskService based on the ESP32 server I have hooked to my desk
        public Task<float> GetHeightAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetPresetAsync(int preset)
        {
            throw new NotImplementedException();
        }

        public bool GetCachedConnectionStatus()
        {
            return _isConnected;
        }

        public async Task<bool> GetConnectionStatusAsync()
        {
            var result = false;

            try
            {
                var settings = _settingsService.Load();
                var ip = settings.DeskIpAddress;
                
                if (string.IsNullOrWhiteSpace(ip))
                {
                    result = false;
                }
                else
                {
                    // Add http:// if missing
                    if (!ip.StartsWith("http://") && !ip.StartsWith("https://"))
                    {
                        ip = "http://" + ip;
                    }

                    // Append trailing slash if missing
                    if (!ip.EndsWith("/"))
                    {
                        ip += "/";
                    }

                    using var response = await _httpClient.GetAsync(ip);
                    result = response.IsSuccessStatusCode;
                }
            }
            catch
            {
                result = false;
            }

            _isConnected = result;
            return result;
        }
    }
}
