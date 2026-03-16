using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeskAppWPF.Messages
{
    public class ConnectionStatusMessage
    {
        public bool IsConnected { get; }

        public ConnectionStatusMessage(bool isConnected)
        {
            IsConnected = isConnected;
        }
    }
}
