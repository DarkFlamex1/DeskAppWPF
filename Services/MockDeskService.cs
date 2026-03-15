using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeskAppWPF.Services
{
    internal class MockDeskService : IDeskService
    {
        public float SimulatedHeight { get; set; } = 75.0f; // Default 75cm

        // Mock service that returns a hard coded height & success/fail for a certain preset
        public Task<float> GetHeightAsync()
        {
            return Task.FromResult(SimulatedHeight);
        }

        public Task<bool> SetPresetAsync(int preset)
        {
            return Task.FromResult(true);
        }
    }
}
