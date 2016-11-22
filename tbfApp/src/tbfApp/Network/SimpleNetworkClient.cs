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
        public delegate void protocolFunction(string prot);

        //--Private
        private TcpSocketClient socket;
        private byte[] buffer {get;}
        private string ip;
        private short port;
        private string network_AKey;
        private event protocolFunction protAnalyseFunction;


        //Constructor
        public SimpleNetworkClient()
        {

        }

        public SimpleNetworkClient(protocolFunction protAnalyseFunction, string network_AKey, string ip, short port)
        {
            this.protAnalyseFunction = protAnalyseFunction;
            this.network_AKey = network_AKey;
            socket = new TcpSocketClient();
            this.ip = ip;
            this.port = port;

        }

        //Functions

        public async Task<bool> connect()
        {
            try
            {
                await socket.ConnectAsync(ip, port);
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
                socket.WriteStream.Write(bytes, 0, bytes.Length);
                //Temporary usage (Must be tested)
                Task.Delay(1000);
                receiveData();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        //Receive function (triggern durch message vom server
        void receiveData()
        {
            while (socket.ReadStream.Read(buffer, 0, buffer.Length) == 0)
            {
                protAnalyseFunction(Encoding.UTF8.GetString(buffer, 0, buffer.Length));
            }
        }

        public async void closeConnection()
        {
            await socket.DisconnectAsync();
        }

        public bool isConnected()
        {
            return false;
        }


       
    }
}