using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Sockets.Plugin;

namespace Network
{
    class SimpleNetworkClient
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
        public SimpleNetworkClient()
        {

        }

        public SimpleNetworkClient(protocolFunction protAnalyseFunction, string network_AKey)
        {
            this.protAnalyseFunction = protAnalyseFunction;
            this.network_AKey = network_AKey;
        }

        public SimpleNetworkClient(protocolFunction protAnalyseFunction, string network_AKey, string ip, short port)
        {
            this.protAnalyseFunction = protAnalyseFunction;
            this.network_AKey = network_AKey;
            endpoint = new socketEndpointCommunication(ip, port);
            endpointCommunicationIsDeclared = true;

        }

        //Functions
        public void setSocketEndpointCommunication(string ip, short port)
        {
            //Enpoint deklaration

            endpointCommunicationIsDeclared = true;
        }

        public bool connect()
        {
            try
            {
                //Connect with endpoint
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
            if (enableEncryption)
            {
                bytes = Encoding.UTF8.GetBytes(message);
            }
            else
            {
                bytes = Encoding.UTF8.GetBytes(message);
            }

            try
            {
                //Send
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        //Receive function (triggern durch message vom server


        public void closeConnection()
        {

        }

        public bool isConnected()
        {
            return false;
        }


        //Classes
        private class socketEndpointCommunication
        {
            //Variables

            public byte[] buffer { get; }


            //Constructor
            public socketEndpointCommunication(string ip, short port)
            {
                {

                    buffer = new byte[255]; //Variable eingabe der Buffer größe????
                }
                //Function
            }

        }
    }
}