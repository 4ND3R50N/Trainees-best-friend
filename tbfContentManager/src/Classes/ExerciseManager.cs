using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WhiteCode.Network;

namespace tbfContentManager.Classes
{
    class ExerciseManager
    {
        readonly SimpleNetwork_Client TCPClient;
        readonly MainContentWindow mainContentWindow;
        DataTable exerciseTable = new DataTable();
        List<string> workoutData = new List<string>();
        readonly Dictionary<string, List<string>> exerciseInformation;
        List<string> keyDelete = new List<string>();
        string IdWorkoutToChange;
        string roomID;
        string workoutID;
        bool IsChanged = false;
        Dictionary<string, List<string>> roomInformation;
        Dictionary<string, List<string>> workoutinformation;
        RoomManager roomManager;
        WorkoutManager workoutManager;

        enum level {Level1 = 1, Level2 = 2, Level3 = 3, Level4 = 4, Level5 = 5};

        public ExerciseManager(ref SimpleNetwork_Client TCPClient, MainContentWindow mainContentWindow, RoomManager roomManager, WorkoutManager workoutManager)
        {
            this.TCPClient = TCPClient;
            this.TCPClient.changeProtocolFunction(Server_response_exerciseManager);
            this.mainContentWindow = mainContentWindow;
            this.workoutinformation = new Dictionary<string, List<string>>();
            this.roomManager = roomManager;
            roomInformation = roomManager.roomInformation;
            this.workoutManager = workoutManager;
            //workoutInformation = workoutManager.workoutInformation;
            
        }

        private void Server_response_exerciseManager(string message)
        {
            MessageBox.Show(message);
        }

    }
}
