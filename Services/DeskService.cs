using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeskAppWPF.Services
{
    internal class DeskService : IDeskService
    {
        // HTTP implementation of IDeskService based on the ESP32 server I have hooked to my desk
        public Task<float> GetHeightAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetPresetAsync(int preset)
        {
            throw new NotImplementedException();
        }
    }
}
