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
        networkServer TcpServer;
        DBEngine DatabaseEngine;
        logWriter Logger;
        List<networkServer.networkClientInterface> ActiveConnections;
        
        private string sAesKey;
        private char   cProtocolDelimiter;

        //Konstruktor
        public ControllerCore(short _iPort, char _cProtocolDelimiter, string _sAesKey, string _sDatabaseDriver,
            string _sDBHostIp, short _sDBPort, string _sDBUser, string _sDBPass, string _sDBDefaultDB, string _sLogPath)
        {
            //Logging initialisations
            Logger = new logWriter(_sLogPath);
            Logger.writeInLog(true, "Logging class initialized!");
            //Database Initialisations
            if(_sDatabaseDriver == "mysql")
            {
                DatabaseEngine = new DBMysqlManager(_sDBHostIp,_sDBUser,_sDBPass,_sDBPort,_sDBDefaultDB);

            }else if(_sDatabaseDriver == "mssql")
            {
                DatabaseEngine = new DBMssqlManager(_sDBHostIp, _sDBUser, _sDBPass, _sDBPort, _sDBDefaultDB);
            }
            //Database test
            if (DatabaseEngine.testDBConnection())
            {
                Logger.writeInLog(true, "Database test successfull!");
            }else
            {
                Logger.writeInLog(true, "ERROR: Database test was not successfull!");
                return;
            }

            //Network Initialisations
            ActiveConnections = new List<networkServer.networkClientInterface>();
            sAesKey = _sAesKey;
            this.cProtocolDelimiter = _cProtocolDelimiter;
            TcpServer = new networkServer(networkProtocol, _sAesKey, IPAddress.Any, _iPort, AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Logger.writeInLog(true, "TCP Server ready for start!");


            //TESTCASE
            //networkServer.networkClientInterface dummy = new networkServer.networkClientInterface();
            //networkProtocol("#104;Anderson2;Lars;Pickelin;miau1234;l.pickelin@web.de", ref dummy);
            //networkProtocol("#102;Anderson2;miau1x234", ref dummy);
        }

        public void start()
        {
            if(TcpServer.startListening())
            {
                Logger.writeInLog(true, "Server has been started successfully!");
            }
            else
            {
                Logger.writeInLog(true, "ERROR: The server was not able to start!");
            }
           
        }
        
        private void networkProtocol(string message, ref networkServer.networkClientInterface relatedClient)
        {
            string sProtocolShortcut = getProtocolShortcut(message);
            // Put data in array
            List<string> lDataList = new List<string>();
            lDataList = getProtocolData(getProtocolMessage(message));

            switch (sProtocolShortcut)
            {
                case "#001":
                    prot_001_TestPackage(ref relatedClient); break;
                case "#003":
                    prot_003_TestPackage(lDataList, ref relatedClient); break;
                case "#102":
                    prot_102_loginUser(lDataList, ref relatedClient); break;
                case "#104":
                    prot_104_registerUser(lDataList, ref relatedClient); break;
               
                default:
                    Logger.writeInLog(true, "Unknown package protocol/data received: " + message);
                    break;
            }
        }

        #region Protocol functions
        //Testpackages
        private void prot_001_TestPackage(ref networkServer.networkClientInterface relatedClient)
        {
            Logger.writeInLog(true, "Message #001 (TESTPACKET_NORMAL) received from a client!");
            TcpServer.sendMessage("#002;Greetings from Controller :)", relatedClient);
            Logger.writeInLog(true, "Answered #002 with the greetings message!");
        }
        private void prot_003_TestPackage(List<string> lDataList, ref networkServer.networkClientInterface relatedClient)
        {
            Logger.writeInLog(true, "Message #003 (TESTPACKET_LARGE) received from a client!");
            string sDataPackage = "#004;";
            int iForCounter = 40;
            for (int i = 0; i < iForCounter; i++)
            {
                sDataPackage = sDataPackage + "Package number: " + (i + 1) + ". This is a large datapackage to test the network tcp/ip receive function. You should get " + iForCounter + " of these packages! Good luck, programmer!\n";
            }
            TcpServer.sendMessage(sDataPackage, relatedClient);
            Logger.writeInLog(true, "Answered #004 with the large datapacket!");
        }
        //login

        //Signup
        private void prot_104_registerUser(List<string> lDataList, ref networkServer.networkClientInterface relatedClient)
        {
            //log
            Logger.writeInLog(true, "Message #104 (SIGNUP) received from a client!");
            //register user
            int iSignUpStatusCode = DatabaseEngine.signUpRegisterUser(lDataList[0], lDataList[1], lDataList[2], lDataList[3], lDataList[4]);
            //send message to client
            TcpServer.sendMessage("#105" + cProtocolDelimiter + iSignUpStatusCode, relatedClient);
            Logger.writeInLog(true, "Answered #105 with SignUpCode "+ iSignUpStatusCode + "!");
        }
        //Login
        private void prot_102_loginUser(List<string> lDataList, ref networkServer.networkClientInterface relatedClient)
        {
            //log
            Logger.writeInLog(true, "Message #102 (LOGIN) received from a client!");
            //Try to login user
            int iUserID = 0;
            int iLoginStatusCode = DatabaseEngine.loginUser(lDataList[0], lDataList[1], ref iUserID);
            //Send message to client
            if(iLoginStatusCode != 1)
            {
                TcpServer.sendMessage("#103" + cProtocolDelimiter + iLoginStatusCode, relatedClient);
            }
            else
            {
                TcpServer.sendMessage("#103" + cProtocolDelimiter + iLoginStatusCode + cProtocolDelimiter + iUserID, relatedClient);
            }
            Logger.writeInLog(true, "Answered #103 with LoginCode " + iLoginStatusCode + ". The user id is "+ iUserID +"!");
        }
        #endregion


        #region Support functions
        private List<string> getProtocolData(string message)
        {
            return message.Split(cProtocolDelimiter).ToList();
        }

        private string getProtocolShortcut(string message)
        {
            return message.Split(cProtocolDelimiter)[0];
        }
        private string getProtocolMessage(string message)
        {
            Console.WriteLine(message.Substring(getProtocolShortcut(message).Length));
            return message.Substring(getProtocolShortcut(message).Length + 1);
        }
        #endregion
        
    }
}
