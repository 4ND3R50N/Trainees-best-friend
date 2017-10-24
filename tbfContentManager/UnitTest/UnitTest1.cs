using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Sockets;
using tbfContentManager.Classes;
using WhiteCode.Network;
using tbfContentManager;
using System.Collections.Generic;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public SimpleNetwork_Client ConnectToTCPTest()
        {
            SimpleNetwork_Client TCPClient = new SimpleNetwork_Client(null, 8000, "", IPAddress.Parse("62.138.6.50"),
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
        public void AddRoomSendTest()
        {
            bool bTest1 = false;
            bool bTest2 = false;

            //Arrange not necessary
            SimpleNetwork_Client TCPClient = ConnectToTCPTest();
            MainContentWindow mainContentWindow = new MainContentWindow(ref TCPClient, "test", 18);
            RoomManager roomManager = new RoomManager(ref TCPClient, mainContentWindow, 18);
            
            if(TCPClient != null)
            {
                //Act
                bTest1 = roomManager.AddRoomSend(2, ";", "Hallo Welt", "http://gehtdichnixan.de/", true, "Unit Test Name", "0");

                bTest2 = roomManager.AddRoomSend(2, ";", "Hallo Welt", "http://gehtdichnixan.de/", true, "", "0");
            }
            //Assert
            Assert.IsTrue(bTest1);
            Assert.IsFalse(bTest2);
        }

        [TestMethod]
        public void AddRoomReceiveTest()
        {
            bool bTest1 = false;
            bool bTest2 = false;

            List<string> messageTestList1 = new List<string>();
            messageTestList1.Add("1");
            messageTestList1.Add("1");
            //messageTestList1[1] = "1";

            List<string> messageTestList2 = new List<string>();
            //messageTestList2[1] = "2";
            messageTestList2.Add("2");
            messageTestList2.Add("2");

            SimpleNetwork_Client TCPClient = ConnectToTCPTest();
            MainContentWindow mainContentWindow = new MainContentWindow(ref TCPClient, "test", 18);
            RoomManager roomManager = new RoomManager(ref TCPClient, mainContentWindow, 18);

            if (TCPClient != null)
            {
                //Act
                bTest1 = roomManager.AddRoomReceive(messageTestList1);

                bTest2 = roomManager.AddRoomReceive(messageTestList2);
            }
            //Assert
            Assert.IsTrue(bTest1);
            Assert.IsFalse(bTest2);
        }

        [TestMethod]
        public void AddWorkoutReceiveTest()
        {
            bool bTest1 = false;
            bool bTest2 = false;

            List<string> messageTestList1 = new List<string>();
            messageTestList1.Add("1");
            messageTestList1.Add("1");
            //messageTestList1[1] = "1";

            List<string> messageTestList2 = new List<string>();
            //messageTestList2[1] = "2";
            messageTestList2.Add("2");
            messageTestList2.Add("2");

            SimpleNetwork_Client TCPClient = ConnectToTCPTest();
            MainContentWindow mainContentWindow = new MainContentWindow(ref TCPClient, "test", 18);
            RoomManager roomManager = new RoomManager(ref TCPClient, mainContentWindow, 18);
            WorkoutManager workoutManager = new WorkoutManager(ref TCPClient, mainContentWindow, roomManager);

            if (TCPClient != null)
            {
                //Act
                bTest1 = workoutManager.AddWorkoutReceive(messageTestList1);

                bTest2 = workoutManager.AddWorkoutReceive(messageTestList2);
            }
            //Assert
            Assert.IsTrue(bTest1);
            Assert.IsFalse(bTest2);
        }

        [TestMethod]
        public void AddWorkoutSendTest()
        {
            bool bTest1 = false;
            bool bTest2 = false;

            //Arrange not necessary
            SimpleNetwork_Client TCPClient = ConnectToTCPTest();
            MainContentWindow mainContentWindow = new MainContentWindow(ref TCPClient, "test", 18);
            RoomManager roomManager = new RoomManager(ref TCPClient, mainContentWindow, 18);
            WorkoutManager workoutManager = new WorkoutManager(ref TCPClient, mainContentWindow, roomManager);

            if (TCPClient != null)
            {
                //Act
                bTest1 = workoutManager.AddWorkoutSend(2, ";", "Hallo Welt", "http://gehtdichnixan.de/", "WorkoutName", "2", "0");

                bTest2 = workoutManager.AddWorkoutSend(2, ";", "Hallo Welt", "http://gehtdichnixan.de/", "", "2", "0");
            }
            //Assert
            Assert.IsTrue(bTest1);
            Assert.IsFalse(bTest2);
        }



    }
}
