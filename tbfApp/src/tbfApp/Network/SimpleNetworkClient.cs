using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;

namespace Network
{
    public class SimpleNetworkClient
    {
        //Variables
        //--Public
        public delegate void protocolFunction(string prot);
        public bool bMessageReceived = false;
        //--Private
        private Socket socket;
        private byte[] byteBuffer { get; }
        private ArraySegment<byte> arrayBuffer {get;}
        private string ip;
        private short port;
        private short bufferSize;
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
            this.ip = ip;
            this.port = port;
            byteBuffer = new byte[bufferSize];
            arrayBuffer = new ArraySegment<byte>(byteBuffer);
            this.bufferSize = bufferSize;
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
                socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);

                //await socket.ConnectAsync(ip, port);
                //socket.Connect(ip, port);
                await socket.ConnectAsync(ip, port);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Socket.Connect failed!");
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


            //socket.WriteStream.Write(bytes, 0, bytes.Length);
            socket.Send(bytes);
            //await Task.Delay(500);

            //socket.ReadStream.Read(buffer, 0, buffer.Length);
            //await socket.ReadStream.ReadAsync(buffer, 0, buffer.Length);
            //await socket.Receive(buffer, SocketFlags.None);
            //socket.Receive(buffer);
            await socket.ReceiveAsync(arrayBuffer, SocketFlags.None);
            /*
            var readEvent = new AutoResetEvent(false);
            var recieveArgs = new SocketAsyncEventArgs()
            {
                UserToken = readEvent
            };
            int totalRecieved = 0;
            recieveArgs.SetBuffer(buffer, totalRecieved, bufferSize - totalRecieved);//Receive bytes from x to total - x, x is the number of bytes already recieved
            socket.ReceiveAsync(recieveArgs);
            */
            //Timer to wait for Answer from Server
            await Task.Delay(new TimeSpan(0, 0, waitingTimeSeconds));
            /*
            protAnalyseFunction(Encoding.UTF8.GetString(byteBuffer, 0, byteBuffer.Length).Replace("\0", string.Empty));
            Array.Clear(byteBuffer, 0, byteBuffer.Length);
            */
            
            protAnalyseFunction(Encoding.UTF8.GetString(arrayBuffer.Array, 0, arrayBuffer.Count()).Replace("\0",string.Empty));
            Array.Clear(arrayBuffer.Array, 0, arrayBuffer.Count());
            

            //Temporary usage (Must be tested)
            //protAnalyseFunction(Encoding.UTF8.GetString(buffer, 0, buffer.Length));
            //bMessageReceived = true;

            return true;
        }

    

        public async void closeConnection()
        {
            //if (isConnected())
            //{
                socket.Shutdown(SocketShutdown.Both); 
                socket.Dispose();
            //}
        }

        public bool isConnected()
        {
            //TODO
            return true;
        }


       
    }
}