using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Threading;

//Global network handling class written by Anderson
//Unneccessary usings for global usement: Protes....


namespace WhiteCode.Network
{
   public class SimpleNetwork_Client
    {
        //Variables
        //--Public
        public bool endpointCommunicationIsDeclared = false;
        public delegate void protocolFunction(string prot);
        //private int iBufferLength;
        //--Private
        private string Network_AKey;
        private socketEndpointCommunication endpoint;        
        private event protocolFunction protAnalyseFunction;
        private Object thisLock = new Object();
        

        //Constructor
        public SimpleNetwork_Client()
        {
            
        }
        public SimpleNetwork_Client(protocolFunction protAnalyseFunction, string network_AKey)
        {
            this.protAnalyseFunction = protAnalyseFunction;
            this.Network_AKey = network_AKey;
        }
        public SimpleNetwork_Client(protocolFunction protAnalyseFunction, int iBufferlength,string network_AKey, string domain, short port,AddressFamily familyType, SocketType socketType, ProtocolType protocolType)
        {
            this.protAnalyseFunction = protAnalyseFunction;
            this.Network_AKey = network_AKey;

            try
            {
                IPAddress ipAddress;
                if (!IPAddress.TryParse(domain, out ipAddress))
                    ipAddress = Dns.GetHostEntry(domain).AddressList[0];

                endpoint = new socketEndpointCommunication(iBufferlength, ipAddress, port, familyType, socketType, protocolType);
                endpointCommunicationIsDeclared = true;
            }
            catch(Exception e)
            {
                MessageBox.Show("Internetadresse des Server konnte nicht aufgelöst werden!");

            }
                       

        }
        
        public void changeProtocolFunction(protocolFunction protAnalyseFunction)
        {
            this.protAnalyseFunction = protAnalyseFunction;
        }

        //Functions
        public void setSocketEndpointCommunication(int iBufferLength, IPAddress ip, short port, AddressFamily familyType, SocketType socketType, ProtocolType protocolType)
        {
            endpoint = new socketEndpointCommunication(iBufferLength, ip, port, familyType, socketType, protocolType);
            endpointCommunicationIsDeclared = true;
        }
        
        public bool connect()
        {
            try
            {
                endpoint.getSocket().Connect(endpoint.clientEndPoint);
                endpoint.getSocket().BeginReceive(endpoint.getBuffer(), 0, endpoint.getBufferLength(), SocketFlags.None, new AsyncCallback(receiveCallback), endpoint);
            }
            catch (Exception e)
            { 
                return false;
            }
            return true;
            
        }

        public bool reloadConnection()
        {
            try
            {
                endpoint.getSocket().Close();
                endpoint.reInitSocket();
                this.connect();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        public bool sendMessage(string message, bool enableEncryption)
        {
            Thread tSend = new Thread(() => {
                byte[] bytes;

                reloadConnection();
                if (enableEncryption) { bytes = Encoding.UTF8.GetBytes(message); }
                else { bytes = Encoding.UTF8.GetBytes(message); }
                try
                {
                    endpoint.getSocket().Send(bytes, bytes.Length, SocketFlags.None);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Probleme mit der Internetverbindung!");
                }
               
            });
            tSend.Start();
            return true;
        }

        private void receiveCallback(IAsyncResult result)
        {
            lock(thisLock)
            {
                socketEndpointCommunication serverMessage = (socketEndpointCommunication)result.AsyncState;
                try
                {
                    int bytestoread = serverMessage.getSocket().EndReceive(result);
                    string text = Encoding.UTF8.GetString(serverMessage.getBuffer(), 0, bytestoread).Replace("\0", string.Empty);

                    if (text != "")
                    {
                        serverMessage.getSocket().BeginReceive(serverMessage.getBuffer(), 0, endpoint.getBufferLength(), SocketFlags.None, new AsyncCallback(receiveCallback), serverMessage);
                        protAnalyseFunction(text);
                        serverMessage.clearBuffer();

                    }
                }
                catch (Exception e)
                {
                    //MessageBox.Show(e.ToString());
                    return;
                }
            }
            
        }

        public void closeConnection()
        {
            endpoint.getSocket().Close();
        }

        public bool isConnected()
        {
            return endpoint.getSocket().Connected;
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
            private Socket socket { get; set; }

            private AddressFamily familyType;
            private SocketType socketType;
            private ProtocolType protocolType;
            private int iBufferLength;
            private byte[] buffer { get; set; } 


            //Constructor
            public socketEndpointCommunication(int bufferLength, IPAddress ip, short port, AddressFamily familyType, SocketType socketType, ProtocolType protocolType)
            {
                this.familyType = familyType;
                this.socketType = socketType;
                this.protocolType = protocolType;
                socket = new Socket(familyType, socketType, protocolType);
                iBufferLength = bufferLength;
                clientEndPoint = new IPEndPoint(ip, port);
                buffer = new byte[bufferLength]; //Variable eingabe der Buffer größe????
            }
            //Function
            public void reInitSocket()
            {
                socket = new Socket(familyType, socketType, protocolType);
            }

            public void clearBuffer()
            {
                buffer = new byte[iBufferLength];

            }

            public int getBufferLength()
            {
                return iBufferLength;
            }
            public Socket getSocket()
            {
                return socket;
            }

            public byte[] getBuffer()
            {
                return buffer;
            }
        }

    }
}
