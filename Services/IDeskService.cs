using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeskAppWPF.Services
{
    internal interface IDeskService
    {
        public Task<float> GetHeightAsync();
        public Task<bool> SetPresetAsync(int preset);
    }
}
