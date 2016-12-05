using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.IO;

//Global network handling class written by Anderson
//Unneccessary usings for global usement: Protes....


namespace WhiteCode.Network
{
   public class simpleNetwork_Client
    {
        //Variables
        //--Public
        public bool endpointCommunicationIsDeclared = false;
        public delegate void protocolFunction(string prot);
        //--Private
        private string network_AKey;
        private socketEndpointCommunication endpoint;        
        private event protocolFunction protAnalyseFunction;


        //Constructor
        public simpleNetwork_Client()
        {

        }
        public simpleNetwork_Client(protocolFunction protAnalyseFunction, string network_AKey)
        {
            this.protAnalyseFunction = protAnalyseFunction;
            this.network_AKey = network_AKey;
        }
        public simpleNetwork_Client(protocolFunction protAnalyseFunction,string network_AKey, IPAddress ip, short port,AddressFamily familyType, SocketType socketType, ProtocolType protocolType)
        {
            this.protAnalyseFunction = protAnalyseFunction;
            this.network_AKey = network_AKey;
            endpoint = new socketEndpointCommunication(ip, port, familyType, socketType, protocolType);
            endpointCommunicationIsDeclared = true;
           
        }
        
        //Functions
        public void setSocketEndpointCommunication(IPAddress ip, short port, AddressFamily familyType, SocketType socketType, ProtocolType protocolType)
        {
            endpoint = new socketEndpointCommunication(ip, port, familyType, socketType, protocolType);
            endpointCommunicationIsDeclared = true;
        }
        
        public bool connect()
        {
            try
            {
                endpoint.socket.Connect(endpoint.clientEndPoint);
                endpoint.socket.BeginReceive(endpoint.buffer, 0, 255, SocketFlags.None, new AsyncCallback(receiveCallback), endpoint);
            }
            catch (Exception)
            { 
                return false;
            }
            return true;
            
        }

        public bool sendMessage(string message, bool enableEncryption)
        {
            byte[] bytes;
            if (enableEncryption) { bytes = Encoding.UTF8.GetBytes(message); }
            else { bytes = Encoding.UTF8.GetBytes(message); }

            try
            {
                endpoint.socket.Send(bytes, bytes.Length, SocketFlags.None);
            }
            catch (Exception)
            {
                return false;
            }
            return true;            
        }

        private void receiveCallback(IAsyncResult result)
        {
            socketEndpointCommunication serverMessage = (socketEndpointCommunication)result.AsyncState;
            try
            {
                int bytestoread = serverMessage.socket.EndReceive(result);
                string text = Encoding.UTF8.GetString(serverMessage.buffer, 0, bytestoread);
                if (bytestoread > 0)
                {
                    serverMessage.socket.BeginReceive(serverMessage.buffer, 0, 255, SocketFlags.None, new AsyncCallback(receiveCallback), serverMessage);
                    protAnalyseFunction(text);
                }
            }
            catch (Exception e)
            {
                return;
            }
        }

        public void closeConnection()
        {
            endpoint.socket.Close();
        }

        public bool isConnected()
        {
            return endpoint.socket.Connected;
        }

        public static string getWebRequest(string URL)
        {
            string fullString;

            WebRequest request = WebRequest.Create(URL);

            ServicePointManager.ServerCertificateValidationCallback +=
            delegate (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                                     System.Security.Cryptography.X509Certificates.X509Chain chain,
                                     System.Net.Security.SslPolicyErrors sslPolicyErrors)
            {
                return true; // **** Always accept
            };

            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(URL);
            HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            //WebRequest request = WebRequest.Create(URL);
            //using (WebResponse response = request.GetResponse())
            //{
            using (StreamReader stream = new StreamReader(httpResponse.GetResponseStream()))
            {
                fullString = stream.ReadToEnd();
            }
            //}
            return fullString;
        } 
        //Classes
        private class socketEndpointCommunication
        {
            //Variables
            public IPEndPoint clientEndPoint { get; }
            public Socket socket { get; }
            public byte[] buffer { get; } 


            //Constructor
            public socketEndpointCommunication(IPAddress ip, short port, AddressFamily familyType, SocketType socketType, ProtocolType protocolType)
            {
                socket = new Socket(familyType, socketType, protocolType);
                clientEndPoint = new IPEndPoint(ip, port);
                buffer = new byte[255]; //Variable eingabe der Buffer größe????
            }
            //Function
        }

    }
}
