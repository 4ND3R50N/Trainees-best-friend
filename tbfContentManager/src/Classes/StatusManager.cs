using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WhiteCode.Network;

namespace tbfContentManager.Classes
{
    class StatusManager
    {
        readonly MainContentWindow mainContentWindow;
        readonly SimpleNetwork_Client TCPClient;

        public StatusManager(ref SimpleNetwork_Client TCPClient, MainContentWindow mainContentWindow)
        {
            this.TCPClient = TCPClient;
            this.TCPClient.changeProtocolFunction(Server_response_statusManager);
            this.mainContentWindow = mainContentWindow;
        }

        private void Server_response_statusManager(string message)
        {
            MessageBox.Show(message);
        }

       
    }
}
