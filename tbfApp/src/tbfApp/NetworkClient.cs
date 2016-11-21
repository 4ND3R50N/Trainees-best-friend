using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sockets.Plugin;

namespace tbfApp
{
    class NetworkClient
    {
        public NetworkClient()
        {
            var address = "127.0.0.1";
            var port = 11000;
            var r = new Random();

            var client = new TcpSocketClient();
            await client.ConnectAsync(address, port);

            // we're connected!
            for (int i = 0; i < 5; i++)
            {
                // write to the 'WriteStream' property of the socket client to send data
                var nextByte = (byte)r.Next(0, 254);
                client.WriteStream.WriteByte(nextByte);
                await client.WriteStream.FlushAsync();

                // wait a little before sending the next bit of data
                await Task.Delay(TimeSpan.FromMilliseconds(500));
            }

            await client.DisconnectAsync();
        }
    }
}
