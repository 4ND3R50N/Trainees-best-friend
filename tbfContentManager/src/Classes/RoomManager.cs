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
    public class RoomManager
    {
        readonly SimpleNetwork_Client TCPClient;
        readonly MainContentWindow mainContentWindow;
        DataTable roomTable = new DataTable();
        List<string> roomData = new List<string>();
        Dictionary<string, List<string>> roomInformation;

        public RoomManager(ref SimpleNetwork_Client TCPClient, MainContentWindow mainContentWindow)
        {
            this.TCPClient = TCPClient;
            this.TCPClient.changeProtocolFunction(Server_response_roomManager);
            this.mainContentWindow = mainContentWindow;
            this.roomInformation = new Dictionary<string, List<string>>();
        }
        
        private void Server_response_roomManager(string message)
        {
            List<string> messageList = new List<string>();

            messageList = message.Split(';').ToList();
            string prot = message.Split(';')[0];
            //MessageBox.Show(message);

            switch (prot)
            {
                case "#202":
                    GetAllRoomInformation(message, messageList);
                    break;

                case "#204":
                    AddRoomReceive(messageList);
                    break;

                default:
                    MessageBox.Show("Server Kommunikationsproblem!");
                    break;
            }
        }

        public bool AddRoomSend(int iUserId, string sTrennzeichen, string sBeschreibung,
            string sPicURL, bool isPrivate, string sRoomName)
        {
            int i_isPrivate_room = 0;
            if (isPrivate)
            {
                i_isPrivate_room = 1;
            }
            else
            {
                i_isPrivate_room = 0;
            }
            if (sRoomName.Length > 0)
            {
                /*WICHTIG FUER SPAETER!
                    Bild muss vorher auf DB geschickt, der schickt dann URL zurueck, dass ist dann die txt_url_room 
                 */
                TCPClient.sendMessage("#203;" + iUserId + sTrennzeichen + sRoomName + sTrennzeichen
                    + sBeschreibung + sTrennzeichen + i_isPrivate_room + sTrennzeichen + sPicURL + sTrennzeichen, true);
                return true;
            }
            return false;
        }

        public void AddRoomReceive(List<string> messageServer)
        {
            if (messageServer[1] == "1")
            {
                MessageBox.Show("Der Raum wurde erfolgreich angelegt!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                GetAllRoomSend();
            }
            else if (messageServer[1] == "2")
            {
                MessageBox.Show("Der Raumname existiert bereits!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        public void GetAllRoomSend()
        {
            TCPClient.sendMessage("#201", true);
        }

        public void GetAllRoomInformation(string message, List<string> messageList)
        {            
            roomTable = new DataTable();
            roomTable.Columns.Add("Key");
            roomTable.Columns.Add("Räume");

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
            mainContentWindow._listView_room.Dispatcher.BeginInvoke((Action)(() => mainContentWindow._listView_room.DataContext = dt));
            mainContentWindow._gridView_room.Dispatcher.BeginInvoke((Action)(() => mainContentWindow._gridView_room.Columns.Clear()));

            Binding bind = new Binding();
            mainContentWindow._listView_room.Dispatcher.BeginInvoke((Action)(() => mainContentWindow._listView_room.SetBinding(ListView.ItemsSourceProperty, bind)));
            //mainContentWindow._listView_room.MouseLeftButtonDown += _listView_room_MouseLeftButtonDown;
            int n = 0;
            foreach (var colum in dt.Columns)
            {
                if (n == 0)
                {
                    n++;
                    continue;
                }
                mainContentWindow._gridView_room.Dispatcher.BeginInvoke((Action)(() => {
                    DataColumn dc = (DataColumn)colum;
                    GridViewColumn column = new GridViewColumn();
                    column.DisplayMemberBinding = new Binding(dc.ColumnName);

                    column.Header = dc.ColumnName;
                    mainContentWindow._listView_room.SelectionChanged += _listView_room_SelectionChanged;
                    mainContentWindow._gridView_room.Columns.Add(column);
                }));
            }
        }

        private void _listView_room_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {          
            DataRowView row = (DataRowView)e.AddedItems[0];
            string key = row.Row["Key"] as string;
            List<string> roomData = new List<string>();

            roomInformation.TryGetValue(key, out roomData);

            bool isPrivate = false;
            if (roomData[3] == "1")
            {
                isPrivate = true;
            }                 

            mainContentWindow.gb_roomInfos.Dispatcher.BeginInvoke((Action)(() => mainContentWindow.gb_roomInfos.Visibility = Visibility.Visible));
            mainContentWindow.btn_saveRoom.Dispatcher.BeginInvoke((Action)(() => mainContentWindow.btn_saveRoom.Visibility = Visibility.Hidden));
            mainContentWindow.btn_saveChangeRoom.Dispatcher.BeginInvoke((Action)(() => mainContentWindow.btn_saveChangeRoom.Visibility = Visibility.Visible));          

            mainContentWindow.txt_name_room.Dispatcher.BeginInvoke((Action)(() => mainContentWindow.txt_name_room.Text = roomData[1]));
            mainContentWindow.txt_beschreibung_room.Dispatcher.BeginInvoke((Action)(() => mainContentWindow.txt_beschreibung_room.Text = roomData[2]));          
            mainContentWindow.b_isPrivate_room.Dispatcher.BeginInvoke((Action)(() => mainContentWindow.b_isPrivate_room.IsChecked = isPrivate));
        }

        public void clearAllTxtFields()
        {
            mainContentWindow.txt_name_room.Text = "";
            mainContentWindow.txt_beschreibung_room.Text = "";
            mainContentWindow.b_isPrivate_room.IsChecked = false;
            mainContentWindow.txt_url_pic_room.Text = "";
        }

        public void addRoomClick() {
            gb_roomInfos.Visibility = Visibility;
            btn_saveRoom.Visibility = Visibility;
            btn_saveChangeRoom.Visibility = Visibility.Hidden;
            clearAllTxtFields();
        }
    }
}
