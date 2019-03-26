using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WhiteCode.Network;

namespace tbfContentManager.Classes
{
    class TraineeManager
    {
        readonly SimpleNetwork_Client TCPClient;
        readonly MainContentWindow mainContentWindow;
        int UserId;
        private int selectedRoomID;
        string sTrennzeichen;

        public List<Trainee> lstOfTrainees;
        public List<Trainee> lstOfChangedTrainees;

        public TraineeManager(ref SimpleNetwork_Client TCPClient, MainContentWindow mainContentWindow, int iUserId)
        {
            this.TCPClient = TCPClient;
            this.TCPClient.changeProtocolFunction(Server_response_traineeManager);

            this.mainContentWindow = mainContentWindow;
            this.UserId = iUserId;
            this.selectedRoomID = 0;
            sTrennzeichen = mainContentWindow.sTrennzeichen;

            lstOfTrainees = new List<Trainee>();
            lstOfChangedTrainees = new List<Trainee>();
        }

        public void Server_response_traineeManager(string message)
        {
            List<string> messageList = new List<string>();

            messageList = message.Split(';').ToList();
            string prot = message.Split(';')[0];
            //MessageBox.Show(message);

            switch (prot)
            {
                case "#212":
                    GetAllRoomInformation(message, messageList);
                    break;

                case "#224":
                    GetAllTraineeInformation(message, messageList);
                    break;

                case "#226":
                    GetAllTraineeInformation(message, messageList);
                    break;
                case "#228":
                    GetRoomTraineesChangedStatusCode(message, messageList);
                    break;

                default:
                    MessageBox.Show("Server Kommunikationsproblem!");

                    mainContentWindow._listView_trainees.Dispatcher.BeginInvoke((Action)(() =>
                        mainContentWindow.grid_traineeManagerTab.IsEnabled = true));
                    break;
            }
        }
        /*
        public void GetAllTraineeSend()
        {
            TCPClient.sendMessage("#223;", true);
        }
        */
        public void SendGetAllTraineeFromRoom(int roomID)
        {
            TCPClient.sendMessage("#225" + sTrennzeichen + roomID.ToString(), true);
        }

        public void SendRoomTraineesChanges()
        {
            if (selectedRoomID <= 0)
            {
                MessageBox.Show("No Room selected!");
            }

            if (lstOfChangedTrainees.Count <= 0)
            {
                MessageBox.Show("Server Kommunikationsproblem!");
            }

            string message = "#227" + sTrennzeichen + selectedRoomID.ToString() + sTrennzeichen + lstOfChangedTrainees.Count;

            foreach (Trainee changedTrainee in lstOfChangedTrainees)
            {
                message = message + sTrennzeichen + changedTrainee.ID;
            }

            TCPClient.sendMessage(message, true);

            mainContentWindow._listView_traineeRoom.SelectedItems.Clear();
        }

        public void GetAllRoomInformation(string message, List<string> messageList)
        {
            List<string> roomData = new List<string>();
            
            DataTable traineeTable = new DataTable();
            traineeTable.Columns.Add("ID");
            traineeTable.Columns.Add("Räume");

            Dictionary<string, List<string>> roomInformation = new Dictionary<string, List<string>>();

            for (int i = 2; i < messageList.Count; i++)
            {
                roomData = messageList.ElementAt(i).Split('|').ToList();

                roomInformation.Add(roomData[0], roomData);

                traineeTable.Rows.Add(roomData.ElementAt(0), roomData.ElementAt(1));
            }

            traineeTable.AcceptChanges();
            LoadTable_Room(traineeTable);
        }

        private void LoadTable_Room(DataTable dt)
        {
            mainContentWindow._listView_traineeRoom.Dispatcher.BeginInvoke((Action)(() => mainContentWindow._listView_traineeRoom.DataContext = dt));
            mainContentWindow._gridView_traineeRoom.Dispatcher.BeginInvoke((Action)(() =>
            {
                for (int i = 1; i < mainContentWindow._gridView_traineeRoom.Columns.Count; i++)
                {
                    mainContentWindow._gridView_traineeRoom.Columns.RemoveAt(i);
                }
            }
             ));

            Binding bind = new Binding();
            mainContentWindow._listView_traineeRoom.Dispatcher.BeginInvoke((Action)(() => mainContentWindow._listView_traineeRoom.SetBinding(ListView.ItemsSourceProperty, bind)));

            int n = 0;
            foreach (var colum in dt.Columns)
            {
                if (n == 0)
                {
                    n++;
                    continue;
                }

                mainContentWindow._gridView_traineeRoom.Dispatcher.BeginInvoke((Action)(() =>
                {
                    DataColumn dc = (DataColumn)colum;
                    GridViewColumn column = new GridViewColumn();
                    column.DisplayMemberBinding = new Binding(dc.ColumnName);
                    column.Header = dc.ColumnName;
                    mainContentWindow._listView_traineeRoom.SelectionChanged += _listView_traineeRoom_SelectionChanged;
                    mainContentWindow._gridView_traineeRoom.Columns.Add(column);
                }));
            }
        }

        private void _listView_traineeRoom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mainContentWindow.bt_add_trainee.IsEnabled = false;
            mainContentWindow.bt_abbrechen_trainee.IsEnabled = false;

            mainContentWindow.grid_traineeManagerTab.IsEnabled = false;
            lstOfTrainees = new List<Trainee>();
            lstOfChangedTrainees = new List<Trainee>();

            if (e.AddedItems.Count != 0)
            {
                DataRowView row = (DataRowView)e.AddedItems[0];
                string key = row.Row["ID"] as string;
                selectedRoomID = Int32.Parse(key);

                SendGetAllTraineeFromRoom(selectedRoomID);
            }
            else
            {
                mainContentWindow._listView_trainees.ItemsSource = lstOfTrainees;
                mainContentWindow.grid_traineeManagerTab.IsEnabled = true;
            }

            e.Handled = true;
        }

        public void GetAllTraineeInformation(string message, List<string> messageList)
        {
            for (int i = 2; i < messageList.Count; i++)
            {
                List<string> traineeData = messageList.ElementAt(i).Split('|').ToList();

                bool isInRoomSelection = false;
                if (!traineeData.ElementAt(3).Equals(""))
                {
                    isInRoomSelection = true;
                }

                int userID = 0;
                try
                {
                    userID = Int32.Parse(traineeData.ElementAt(0));
                    Console.WriteLine(userID);
                }
                catch (FormatException)
                {
                    Console.WriteLine($"Unable to parse '{traineeData.ElementAt(0)}'");
                }

                Trainee trainee = new Trainee(userID, isInRoomSelection, traineeData.ElementAt(1), traineeData.ElementAt(2));

                lstOfTrainees.Add(trainee);
            }
            
            mainContentWindow._listView_trainees.Dispatcher.BeginInvoke((Action) (() =>
                mainContentWindow._listView_trainees.ItemsSource = lstOfTrainees));
                
            mainContentWindow.grid_traineeManagerTab.Dispatcher.BeginInvoke((Action)(() =>
                mainContentWindow.grid_traineeManagerTab.IsEnabled = true));
        }

        public void GetRoomTraineesChangedStatusCode(string message, List<string> messageList)
        {
            if (messageList[1].Equals("1"))
            {
                MessageBox.Show("Änderungen der Trainees am Raum erfolgreich durchgeführt");
            }
            else
            {
                MessageBox.Show("Fehlgeschlagen, Änderungen konnten nicht durchgeführt werden");
            }
        }

        public void traineeCheckBoxChanged(Object sender)
        {
            mainContentWindow.Dispatcher.BeginInvoke((Action)(() =>
                {
                    mainContentWindow.bt_add_trainee.IsEnabled = true;
                    mainContentWindow.bt_abbrechen_trainee.IsEnabled = true;
                }));

            CheckBox checkBox = (CheckBox)sender;

            int userID = 0;
            try
            {
                userID = Int32.Parse(checkBox.Tag.ToString());
                Console.WriteLine(userID);
            }
            catch (FormatException)
            {
                Console.WriteLine($"Unable to parse '{checkBox.Tag.ToString()}'");
            }

            Trainee changedTrainee = lstOfTrainees.Find(item => item.ID == userID);

            if (!lstOfChangedTrainees.Contains(changedTrainee))
            {
                lstOfChangedTrainees.Add(changedTrainee);
            }
            else
            {
                lstOfChangedTrainees.Remove(changedTrainee);
            }
        }
    }
}