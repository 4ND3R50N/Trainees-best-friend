using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleNetworkServer;
using Support;
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
        logWriter logger;
        List<networkServer.networkClientInterface> activeConnections;
        
        private string sAesKey;


        //Konstruktor
        public ControllerCore(short _iPort, string _sAesKey, string _sDatabaseDriver,
            string _sDBHostIp, short _sDBPort, string _sDBUser, string _sDBPass, string _sDBDefaultDB, string _sLogPath)
        {
            //Logging initialisations
            logger = new logWriter(_sLogPath);
            logger.writeInLog(true, "Logging class initialized!");
            //Database Initialisations
            if(_sDatabaseDriver == "mysql")
            {
                databaseEngine = new DBMysqlManager(_sDBHostIp,_sDBUser,_sDBPass,_sDBPort,_sDBDefaultDB);

            }else if(_sDatabaseDriver == "mssql")
            {
                databaseEngine = new DBMssqlManager(_sDBHostIp, _sDBUser, _sDBPass, _sDBPort, _sDBDefaultDB);
            }

            if (databaseEngine.testDBConnection())
            {
                logger.writeInLog(true, "Database test successfull!");
            }else
            {
                logger.writeInLog(true, "ERROR: Database test was not successfull!");
                return;
            }

            //Network Initialisations
            activeConnections = new List<networkServer.networkClientInterface>();
            sAesKey = _sAesKey;
            tcpServer = new networkServer(networkProtocol, _sAesKey, IPAddress.Any, _iPort, AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            logger.writeInLog(true, "TCP Server ready for start!");           
        }

        public void start()
        {
            if(tcpServer.startListening())
            {
                logger.writeInLog(true, "Server has been started successfully!");
            }
            else
            {

                logger.writeInLog(true, "ERROR: The server was not able to start!");
            }
           
        }
        
        private void networkProtocol(string message, ref networkServer.networkClientInterface relatedClient)
        {
            string protocolShortcut = getProtocolShortcut(message);
            string realMessage = getProtocolMessage(message);
            Console.WriteLine("["+ DateTime.Now + "] system@tbf-controller: Data revceived: " + message);
            switch (protocolShortcut)
            {
                case "#101":
                    Console.WriteLine("[" + DateTime.Now + "] system@tbf-controller: Got #101, send answer back!");
                    string test = "";
                    for (int i = 0; i < 20; i++)
                    {
                        test = test + "Daten Packet. Xamarin + Controller + Content manager = Cool!---------------------------------------------------------------------------------fhgfchfhffhfvfgvchfhvhfhvfhvfhvhvfhfzzree5e5frzrfzrzfrzrfzrtzrzrzrzrzrzzrzrvrtreteswrwr\n";
                    }
                    tcpServer.sendMessage(test, relatedClient);
                    break;
                default:
                    Console.WriteLine(message);
                    break;
            }
        }

        #region Support functions
        private string getProtocolShortcut(string message)
        {
            return message.Substring(0, 4);
        }
        private string getProtocolMessage(string message)
        {
            return message.Substring(4);
        }
        #endregion



    }
}
