using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WhiteCode.Network;

namespace tbfContentManager.Classes
{
    public class ExerciseManager
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
        Dictionary<string, List<string>> workoutInformation;
        RoomManager roomManager;
        WorkoutManager workoutManager;

        enum level {Level1 = 1, Level2 = 2, Level3 = 3, Level4 = 4, Level5 = 5};

        public ExerciseManager(ref SimpleNetwork_Client TCPClient, MainContentWindow mainContentWindow, RoomManager roomManager, WorkoutManager workoutManager)
        {
            this.TCPClient = TCPClient;
            this.TCPClient.changeProtocolFunction(Server_response_exerciseManager);
            this.mainContentWindow = mainContentWindow;
            this.workoutInformation = new Dictionary<string, List<string>>();
            this.roomManager = roomManager;
            roomInformation = roomManager.roomInformation;
            this.workoutManager = workoutManager;

            mainContentWindow.cb_roomChoose_Exercise.SelectionChanged += Cb_roomChoose_Exercise_SelectionChanged;
        }

        public void Server_response_exerciseManager(string message)
        {
            MessageBox.Show(message);
            List<string> messageList = new List<string>();

            messageList = message.Split(';').ToList();
            string prot = message.Split(';')[0];
            //MessageBox.Show(message);

            switch (prot)
            {
                case "#2":
                    //MessageBox.Show(message);
                    break;

                case "#21":
                    break;

                case "#22":
                    break;

                default:
                    MessageBox.Show("Server Kommunikationsproblem!");
                    break;
            }

        }

        public void ShowAllRooms() {
            mainContentWindow.cb_roomChoose_Exercise.Dispatcher.BeginInvoke((Action)(() =>
            {

                if (mainContentWindow.cb_roomChoose_Exercise.Items.Count > 0)
                {
                    mainContentWindow.cb_roomChoose_Exercise.Items.Clear();
                }

                mainContentWindow.cb_roomChoose_Exercise.Items.Add("Bitte Raum auswählen!");
                mainContentWindow.cb_roomChoose_Exercise.SelectedIndex = 0;

                for (int i = 0; i < roomInformation.Count; i++)
                {
                    mainContentWindow.cb_roomChoose_Exercise.Items.Add(roomInformation.ElementAt(i).Value.ElementAt(1));
                }

            }));
        }

        private void Cb_roomChoose_Exercise_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 0)
            {
                mainContentWindow.gb_ExerciseInfos.Visibility = Visibility.Hidden;

                if (mainContentWindow.cb_roomChoose_Exercise.SelectedIndex == 0)
                {
                    mainContentWindow._listView_workout.Visibility = Visibility.Hidden;
                }
                else
                {
                    for (int i = 0; i < roomInformation.Count; i++)
                    {
                        if (e.AddedItems[0].ToString() == roomInformation.ElementAt(i).Value.ElementAt(1))
                        {
                            mainContentWindow._listView_workout.Visibility = Visibility.Visible;
                            roomID = roomInformation.ElementAt(i).Value.ElementAt(0);
                            Console.WriteLine(roomID + "das war roomID");
                            string roomName = roomInformation.ElementAt(i).Value.ElementAt(1);
                            mainContentWindow.cbChooseARoom_Workout.Content = roomName;
                            ShowAllWorkouts(roomID);
                        }
                    }
                }
            }
        }

        private void ShowAllWorkouts(string roomID) {
            TCPClient.changeProtocolFunction(workoutManager.Server_response_workoutManager);
            workoutManager.GetAllWorkoutSend(roomID);
            Thread.Sleep(400);

            workoutInformation = workoutManager.workoutInformation;

            //Console.WriteLine(workoutInformation);

            //mainContentWindow.tb_workoutShow_Exercise.Visibility = Visibility.Visible;

            //mainContentWindow.cb_workoutChoose_Exercise.Dispatcher.BeginInvoke((Action)(() =>
            //{

            //    if (mainContentWindow.cb_workoutChoose_Exercise.Items.Count > 0)
            //    {
            //        mainContentWindow.cb_workoutChoose_Exercise.Items.Clear();
            //    }

            //    mainContentWindow.cb_workoutChoose_Exercise.Items.Add("Bitte Raum auswählen!");
            //    mainContentWindow.cb_workoutChoose_Exercise.SelectedIndex = 0;

            //    for (int i = 0; i < workoutInformation.Count; i++)
            //    {
            //        string test = workoutInformation.ElementAt(i).Value.ElementAt(1);
            //        //Console.WriteLine(test);
            //        mainContentWindow.cb_workoutChoose_Exercise.Items.Add(workoutInformation.ElementAt(i).Value.ElementAt(1));
            //    }

            //}));




        }



    }
}
