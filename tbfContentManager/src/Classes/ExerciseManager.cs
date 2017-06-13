using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WhiteCode.Network;

namespace tbfContentManager.Classes
{
    class ExerciseManager
    {
        readonly MainContentWindow mainContentWindow;
        readonly SimpleNetwork_Client TCPClient;

        public ExerciseManager(ref SimpleNetwork_Client TCPClient, MainContentWindow mainContentWindow)
        {
            this.TCPClient = TCPClient;
            this.TCPClient.changeProtocolFunction(Server_response_exerciseManager);
            this.mainContentWindow = mainContentWindow;
        }

        private void Server_response_exerciseManager(string message)
        {
            MessageBox.Show(message);
        }

    }
}
