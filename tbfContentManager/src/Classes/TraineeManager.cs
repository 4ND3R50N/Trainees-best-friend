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
        DataTable roomTable = new DataTable();
        List<string> roomData = new List<string>();
        public Dictionary<string, List<string>> roomInformation;
        List<string> keyDelete = new List<string>();
        string IdRoomToChange;
        bool IsChanged = false;
        int UserId;

        public TraineeManager(ref SimpleNetwork_Client TCPClient, MainContentWindow mainContentWindow, int iUserId)
        {
            this.TCPClient = TCPClient;
            this.TCPClient.changeProtocolFunction(Server_response_traineeManager);
            this.mainContentWindow = mainContentWindow;
            this.roomInformation = new Dictionary<string, List<string>>();
            this.UserId = iUserId;
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

                default:
                    MessageBox.Show("Server Kommunikationsproblem!");
                    break;
            }
        }

        public void GetAllRoomInformation(string message, List<string> messageList)
        {
            roomTable = new DataTable();
            roomTable.Columns.Add("ID");
            roomTable.Columns.Add("Räume");
            roomInformation.Clear();
            for (int i = 2; i < messageList.Count; i++)
            {
                roomData = messageList.ElementAt(i).Split('|').ToList();

                roomInformation.Add(roomData[0], roomData);

                roomTable.Rows.Add(roomData.ElementAt(0), roomData.ElementAt(1));
            }

            roomTable.AcceptChanges();
            LoadTable_Room(roomTable);
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
            /*
            if (e.AddedItems.Count != 0)
            {
                DataRowView row = (DataRowView)e.AddedItems[0];
                string key = row.Row["ID"] as string;
                List<string> roomDataInformation = new List<string>();

                IdRoomToChange = key;

                roomInformation.TryGetValue(key, out roomDataInformation);

                bool isPrivate = false;
                if (roomDataInformation[3] == "1")
                {
                    isPrivate = true;
                }

                mainContentWindow.gb_roomInfos.Visibility = Visibility.Visible;
                mainContentWindow.btn_saveRoom.Visibility = Visibility.Hidden;
                mainContentWindow.btn_saveChangeRoom.Visibility = Visibility.Visible;
                mainContentWindow.btn_deleteRoom.Visibility = Visibility.Visible;

                mainContentWindow.txt_name_room.Text = roomDataInformation[1];
                mainContentWindow.txt_beschreibung_room.Text = roomDataInformation[2];
                mainContentWindow.b_isPrivate_room.IsChecked = isPrivate;
                mainContentWindow.txt_url_pic_room.Text = roomDataInformation[4];
            }
            */
        }
    }
}
