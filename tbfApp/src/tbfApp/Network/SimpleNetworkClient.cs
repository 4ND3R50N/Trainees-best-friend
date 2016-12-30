using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Sockets.Plugin;
using System.Diagnostics;
using Sockets.Plugin.Abstractions;

namespace Network
{
    public class SimpleNetworkClient
    {
        //Variables
        //--Public
        public delegate void protocolFunction(string prot);
        public bool bMessageReceived = false;
        //--Private
        private TcpSocketClient socket;
        private byte[] buffer {get;}
        private string ip;
        private short port;
        private string network_AKey;
        private short waitingTimeSeconds;
        private event protocolFunction protAnalyseFunction;


        //Constructor
        public SimpleNetworkClient()
        {

        }

        public SimpleNetworkClient(protocolFunction protAnalyseFunction, string network_AKey, string ip, short port, short bufferSize, short waitingTimeSeconds)
        {
            this.network_AKey = network_AKey;
            socket = new TcpSocketClient();
            this.ip = ip;
            this.port = port;
            buffer = new byte[bufferSize];
            this.waitingTimeSeconds = waitingTimeSeconds;
        }

        //Functions

        public void SetProtocolFunction(protocolFunction protAnalyseFunction)
        {
            this.protAnalyseFunction = protAnalyseFunction;
        }

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

        public async Task<bool> sendMessage(string message, bool enableEncryption)
        {
            byte[] bytes;
            bMessageReceived = false;

            if (enableEncryption)
            {
                bytes = Encoding.UTF8.GetBytes(message);
            }
            else
            {
                bytes = Encoding.UTF8.GetBytes(message);
            }

            
            socket.WriteStream.Write(bytes, 0, bytes.Length);
            //await Task.Delay(500);
           
            await socket.ReadStream.ReadAsync(buffer, 0, buffer.Length);


            await Task.Delay(new TimeSpan(0, 0, waitingTimeSeconds));
            protAnalyseFunction(Encoding.UTF8.GetString(buffer, 0, buffer.Length).Replace("\0",string.Empty));
   
           
            //Temporary usage (Must be tested)
            //protAnalyseFunction(Encoding.UTF8.GetString(buffer, 0, buffer.Length));
            //bMessageReceived = true;

            return true;
        }

    
        
        public async void closeConnection()
        {
            //if (isConnected())
            //{
                await socket.DisconnectAsync();
            //}
        }

        public bool isConnected()
        {
            //TODO
            return true;
        }


       
    }
}