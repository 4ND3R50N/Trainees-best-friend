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
        List<networkServer.networkClientInterface> activeConnections;
        private string sAesKey;


        //Konstruktor
        public ControllerCore(short _iPort, string _sAesKey, string _sDatabaseDriver,
            string _sDBHostIp, short _sDBPort, string _sDBUser, string _sDBPass, string _sDBDefaultDB)
        {
            //Logging initialisations

            //Database Initialisations
            if(_sDatabaseDriver == "mysql")
            {
                databaseEngine = new DBMysqlManager(_sDBHostIp,_sDBUser,_sDBPass,_sDBPort,_sDBDefaultDB);
            }else if(_sDatabaseDriver == "mssql")
            {
                databaseEngine = new DBMssqlManager(_sDBHostIp, _sDBUser, _sDBPass, _sDBPort, _sDBDefaultDB);
            }else
            {
                
            }

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
