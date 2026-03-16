using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DeskAppWPF.Messages;
using DeskAppWPF.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeskAppWPF.ViewModels
{
    internal partial class ConnectionViewModel : ObservableRecipient, IRecipient<ConnectionStatusMessage>
    {
        [ObservableProperty]
        private string _connectionText = "Disconnected";

        [ObservableProperty]
        private string _connectionColor = "#D13438"; // Red default

        public ConnectionViewModel(IDeskService deskService)
        {
            // Register to receive connection status messages
            IsActive = true;

            // use the cached status
            UpdateConnectionText(deskService.GetCachedConnectionStatus());
        }

        public void Receive(ConnectionStatusMessage message)
        {
            UpdateConnectionText(message.IsConnected);
        }

        private void UpdateConnectionText(bool isConnected)
        {
            // Update properties on the UI thread
            App.Current.Dispatcher.Invoke(() =>
            {
                if (isConnected)
                {
                    ConnectionText = "Connected";
                    ConnectionColor = "#107C10"; // Green
                }
                else
                {
                    ConnectionText = "Disconnected";
                    ConnectionColor = "#D13438"; // Red
                }
            });
        }
    }
}
