using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Sockets;
using tbfContentManager.Classes;
using WhiteCode.Network;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public simpleNetwork_Client ConnectToTCP()
        {
            simpleNetwork_Client TCPClient = new simpleNetwork_Client(null, "", IPAddress.Parse("62.138.6.50"),
                                                13001, AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            if(TCPClient.connect())
            {
                Assert.IsTrue(true);
                return TCPClient;
            }
            else {
                Assert.IsFalse(false);
                return null;
            }
            
        }



       [TestMethod]
        public void addRoomSend()
        {
            bool bTest1 = false;
            bool bTest2 = false;


            //Arrange not necessary
            simpleNetwork_Client TCPClient = ConnectToTCP();
            
            if(TCPClient != null)
            {
                //Act
                bTest1 = RoomManager.AddRoomSend(ref TCPClient, 2, ";", "Hallo Welt", "http://gehtdichnixan.de/", true, "Unit Test Name");

                bTest2 = RoomManager.AddRoomSend(ref TCPClient, 2, ";", "Hallo Welt", "http://gehtdichnixan.de/", true, "");
            }
            //Assert
            Assert.IsTrue(bTest1);
            Assert.IsFalse(bTest2);
        }
    }
}
