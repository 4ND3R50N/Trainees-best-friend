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
    class WorkoutManager
    {
        readonly SimpleNetwork_Client TCPClient;
        readonly MainContentWindow mainContentWindow;
        DataTable workoutTable = new DataTable();
        List<string> workoutData = new List<string>();
        readonly Dictionary<string, List<string>> workoutInformation;
        List<string> keyDelete = new List<string>();
        string IdWorkoutToChange;
        bool IsChanged = false;
        Dictionary<string, List<string>> roomInformation;

        RoomManager roomManager;

   

        public WorkoutManager(ref SimpleNetwork_Client TCPClient, MainContentWindow mainContentWindow, RoomManager roomManager)
        {
            this.TCPClient = TCPClient;
           // this.TCPClient.changeProtocolFunction(Server_response_workoutManager);
            this.mainContentWindow = mainContentWindow;
            this.workoutInformation = new Dictionary<string, List<string>>();
            this.roomManager = roomManager;
            roomInformation = roomManager.RoomInformation;
        }

        public void Server_response_workoutManager(string message)
        {        

            List<string> messageList = new List<string>();

            messageList = message.Split(';').ToList();
            string prot = message.Split(';')[0];
            //MessageBox.Show(message);

            switch (prot)
            {
                case "#1":
                    break;

                case "#2":
                    break;

                default:
                    MessageBox.Show("Server Kommunikationsproblem!");
                    break;
            }
        }

        public void ClearAllTxtFields()
        {
            mainContentWindow.txt_name_workout.Text = "";
            mainContentWindow.txt_beschreibung_workout.Text = "";
            mainContentWindow.txt_url_pic_workout.Text = "";
            // hier noch die Raeume!!!!
        }

        public void AddWorkoutClick()
        {
            mainContentWindow.gb_workoutInfos.Visibility = Visibility.Visible;
            mainContentWindow.btn_saveWorkout.Visibility = Visibility.Visible;
            mainContentWindow.btn_cancelWorkout.Visibility = Visibility.Visible;
            mainContentWindow.btn_saveChangeWorkout.Visibility = Visibility.Hidden;
            mainContentWindow.btn_deleteWorkout.Visibility = Visibility.Hidden;
            ClearAllTxtFields();
        }

        public bool AddWorkoutSend(int iUserId, string sTrennzeichen, string sBeschreibung, string sPicURL, string sWorkoutName, string IdWorkout, string RoomId)
        {
            if (sWorkoutName.Length > 0)
            {
                /*WICHTIG FUER SPAETER!
                    Bild muss vorher auf DB geschickt, der schickt dann URL zurueck, dass ist dann die txt_url_workout
                 */
                //TCPClient.sendMessage("#203;" + IdWorkout + sTrennzeichen + iUserId + sTrennzeichen + sWorkoutName + sTrennzeichen
                //    + sBeschreibung + sTrennzeichen + sPicURL + sTrennzeichen, true);
                return true;
            }
            return false;
        }

        public void AddWorkoutReceive(List<string> messageServer)
        {
            if (messageServer[1] == "1")
            {
                if (IsChanged)
                {
                    MessageBox.Show("Der Workout wurde erfolgreich geändert!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    IsChanged = false;
                }
                else
                {
                    MessageBox.Show("Der Workout wurde erfolgreich angelegt!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                mainContentWindow.btn_saveWorkout.Dispatcher.BeginInvoke((Action)(() => mainContentWindow.btn_saveWorkout.Visibility = Visibility.Hidden));
                mainContentWindow.btn_cancelWorkout.Dispatcher.BeginInvoke((Action)(() => mainContentWindow.btn_cancelWorkout.Visibility = Visibility.Hidden));
                mainContentWindow.btn_saveChangeWorkout.Dispatcher.BeginInvoke((Action)(() => mainContentWindow.btn_saveChangeWorkout.Visibility = Visibility.Visible));
                mainContentWindow.btn_deleteWorkout.Dispatcher.BeginInvoke((Action)(() => mainContentWindow.btn_deleteWorkout.Visibility = Visibility.Visible));
            }
            else if (messageServer[1] == "2")
            {
                MessageBox.Show("Der Workoutname existiert bereits!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (messageServer[1] == "3")
            {
                MessageBox.Show("Der Server hat einen internen Fehler! Bitte kontaktieren Sie einen Administrator!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Unbekannter Protokolfehler!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void GetAllWorkoutSend()
        {
            TCPClient.sendMessage("#201", true);
        }

        public void GetAllWorkoutInformation(string message, List<string> messageList)
        {
            workoutTable = new DataTable();
            workoutTable.Columns.Add("ID");
            workoutTable.Columns.Add("Workouts");
            workoutTable.Columns.Add("Raum");
            workoutInformation.Clear();
            for (int i = 2; i < messageList.Count; i++)
            {
                workoutData = messageList.ElementAt(i).Split('|').ToList();

                workoutInformation.Add(workoutData[0], workoutData);

                workoutTable.Rows.Add(workoutData.ElementAt(0), workoutData.ElementAt(1));
            }

            workoutTable.AcceptChanges();
            LoadTable_Workout(workoutTable);
        }

        private void LoadTable_Workout(DataTable dt)
        {
            mainContentWindow._listView_workout.Dispatcher.BeginInvoke((Action)(() => mainContentWindow._listView_workout.DataContext = dt));
            mainContentWindow._gridView_workout.Dispatcher.BeginInvoke((Action)(() =>
            {
                //mainContentWindow._gridView_workout.Columns.Clear())
                //Soo schöner Cooode
                for (int i = 1; i < mainContentWindow._gridView_workout.Columns.Count; i++)
                {   //super funktional, traumhaft
                    mainContentWindow._gridView_workout.Columns.RemoveAt(i);
                }
            }
             ));

            Binding bind = new Binding();
            mainContentWindow._listView_workout.Dispatcher.BeginInvoke((Action)(() => mainContentWindow._listView_workout.SetBinding(ListView.ItemsSourceProperty, bind)));

            int n = 0;
            foreach (var colum in dt.Columns)
            {
                if (n == 0)
                {
                    n++;
                    continue;
                }

                mainContentWindow._gridView_workout.Dispatcher.BeginInvoke((Action)(() =>
                {
                    DataColumn dc = (DataColumn)colum;
                    GridViewColumn column = new GridViewColumn();
                    column.DisplayMemberBinding = new Binding(dc.ColumnName);
                    column.Header = dc.ColumnName;
                    mainContentWindow._listView_workout.SelectionChanged += _listView_workout_SelectionChanged;
                    mainContentWindow._gridView_workout.Columns.Add(column);
                }));
            }
        }

        private void _listView_workout_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 0)
            {
                DataRowView row = (DataRowView)e.AddedItems[0];
                string key = row.Row["ID"] as string;
                List<string> workoutDataInformation = new List<string>();

                IdWorkoutToChange = key;

                workoutInformation.TryGetValue(key, out workoutDataInformation);

                bool isPrivate = false;
                if (workoutDataInformation[3] == "1")
                {
                    isPrivate = true;
                }

                mainContentWindow.gb_workoutInfos.Visibility = Visibility.Visible;
                mainContentWindow.btn_saveWorkout.Visibility = Visibility.Hidden;
                mainContentWindow.btn_saveChangeWorkout.Visibility = Visibility.Visible;
                mainContentWindow.btn_deleteWorkout.Visibility = Visibility.Visible;

                mainContentWindow.txt_name_workout.Text = workoutDataInformation[1];
                mainContentWindow.txt_beschreibung_workout.Text = workoutDataInformation[2];
            }

            foreach (DataRowView deleteKeyFormList in e.RemovedItems)
            {
                //MessageBox.Show(test.Row["ID"] as string);
                keyDelete.Remove(deleteKeyFormList.Row["ID"] as string);
            }
            foreach (DataRowView addKeyToList in e.AddedItems)
            {
                //MessageBox.Show(test.Row["ID"] as string);
                keyDelete.Add(addKeyToList.Row["ID"] as string);
            }
        }

        public void ChangeWorkoutSend(int iUserId, string sTrennzeichen, string sBeschreibung, string sPicURL, string sWorkoutName, string IdWorkout, string RoomId)
        {
            AddWorkoutSend(iUserId, sTrennzeichen, sBeschreibung, sPicURL, sWorkoutName, IdWorkout, RoomId);
            IsChanged = true;
        }

        public void DeleteWorkout()
        {
            string sDeleteWorkout = "";
            string deleteWorkoutTrennzeichen = "|";
            foreach (string tmp in keyDelete)
            {
                sDeleteWorkout += deleteWorkoutTrennzeichen + tmp;

                //MessageBox.Show(tmp);               
            }

            //MessageBox.Show(sDeleteRoom);
        }
    }
}
