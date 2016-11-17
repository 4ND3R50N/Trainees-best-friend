using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleNetworkServer;
using WCDatabaseEngine;
using System.Net;
using System.Net.Sockets;


namespace tbfController.Classes.Core
{
    class ControllerCore
    {

        //Variablen
        networkServer tcpServer;
        DBEngine databaseEngine;
        private string sAesKey;


        //Konstruktor
        public ControllerCore(short _iPort, string _sAesKey)
        {
            //Logging initialisations

            //Database Initialisations

            //Network Initialisations
            activeConnections = new List<networkServer.networkClientInterface>();
            sAesKey = _sAesKey;
            tcpServer = new networkServer(networkProtocol, _sAesKey, IPAddress.Any, _iPort, AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            
        }

        public void start()
        {
            tcpServer.startListening();
            
        }
        
        private void networkProtocol(string message, ref networkServer.networkClientInterface relatedClient)
        {
            string protocolShortcut = getProtocolShortcut(message);
            Console.WriteLine(protocolShortcut);
            switch (protocolShortcut)
            {
                case "#101":

                    break;
                default:
                    Console.WriteLine(message);
                    break;
            }
        }

        #region Support functions
        private string getProtocolShortcut(string message)
        {
            return message.Substring(0, 3);
        }
        #endregion



    }
}
