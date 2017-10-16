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
        public Dictionary<string, List<string>> roomInformation;
        List<string> keyDelete = new List<string>();
        string IdRoomToChange;
        bool IsChanged = false;
        int UserId;

        //public Dictionary<string, List<string>> RoomInformation { get  => roomInformation; }

        public RoomManager(ref SimpleNetwork_Client TCPClient, MainContentWindow mainContentWindow, int iUserId)
        {
            this.TCPClient = TCPClient;
            this.TCPClient.changeProtocolFunction(Server_response_roomManager);
            this.mainContentWindow = mainContentWindow;
            this.roomInformation = new Dictionary<string, List<string>>();
            this.UserId = iUserId;
        }
        
        public void Server_response_roomManager(string message)
        {            
            List<string> messageList = new List<string>();

            messageList = message.Split(';').ToList();
            string prot = message.Split(';')[0];
            //MessageBox.Show(message);

            switch (prot)
            {
                //case "#202":
                //    //MessageBox.Show(message);
                //    GetAllRoomInformation(message, messageList);
                //    break;

                case "#204":
                    AddRoomReceive(messageList);
                    break;

                case "#212":
                    GetAllRoomInformation(message, messageList);
                    break;
                case "#214":
                    DeleteRoomReceive(messageList);
                    break;

                default:
                    MessageBox.Show("Server Kommunikationsproblem!");
                    break;
            }
        }

        public bool AddRoomSend(int iUserId, string sTrennzeichen, string sBeschreibung,
            string sPicURL, bool isPrivate, string sRoomName, string Id)
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
                TCPClient.sendMessage("#203;" + Id + sTrennzeichen + iUserId + sTrennzeichen + sRoomName + sTrennzeichen
                    + sBeschreibung + sTrennzeichen + i_isPrivate_room + sTrennzeichen + sPicURL + sTrennzeichen, true);
                return true;
            }
            return false;
        }

        public bool AddRoomReceive(List<string> messageServer)
        {           
            if (messageServer[1] == "1")
            {
                if (IsChanged)
                {
                    MessageBox.Show("Der Raum wurde erfolgreich geändert!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    IsChanged = false;
                }
                else
                {
                    MessageBox.Show("Der Raum wurde erfolgreich angelegt!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                mainContentWindow.btn_saveRoom.Dispatcher.BeginInvoke((Action)(() => mainContentWindow.btn_saveRoom.Visibility = Visibility.Hidden));
                mainContentWindow.btn_cancelRoom.Dispatcher.BeginInvoke((Action)(() => mainContentWindow.btn_cancelRoom.Visibility = Visibility.Hidden));
                mainContentWindow.btn_saveChangeRoom.Dispatcher.BeginInvoke((Action)(() => mainContentWindow.btn_saveChangeRoom.Visibility = Visibility.Visible));
                mainContentWindow.btn_deleteRoom.Dispatcher.BeginInvoke((Action)(() => mainContentWindow.btn_deleteRoom.Visibility = Visibility.Visible));
                return true;
            }
            else if (messageServer[1] == "2")
            {
                MessageBox.Show("Der Raumname existiert bereits!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                return false;
            }
            else if (messageServer[1] == "3")
            {
                MessageBox.Show("Der Server hat einen internen Fehler! Bitte kontaktieren Sie einen Administrator!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                return false;
            }
            else
            {
                MessageBox.Show("Unbekannter Protokolfehler!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

        }

        public void GetAllRoomSend()
        {
            TCPClient.sendMessage("#211;" + UserId.ToString(), true);
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
            mainContentWindow._listView_room.Dispatcher.BeginInvoke((Action)(() => mainContentWindow._listView_room.DataContext = dt));
            mainContentWindow._gridView_room.Dispatcher.BeginInvoke((Action)(() =>
            {
                for (int i = 1; i < mainContentWindow._gridView_room.Columns.Count; i++)
                {                       
                    mainContentWindow._gridView_room.Columns.RemoveAt(i);
                }
            }
             ));
           
            Binding bind = new Binding();
            mainContentWindow._listView_room.Dispatcher.BeginInvoke((Action)(() => mainContentWindow._listView_room.SetBinding(ListView.ItemsSourceProperty, bind)));

            int n = 0;
            foreach (var colum in dt.Columns)
            {
                if (n == 0)
                {
                    n++;
                    continue;
                }

                mainContentWindow._gridView_room.Dispatcher.BeginInvoke((Action)(() =>
                {                   
                    DataColumn dc = (DataColumn)colum;
                    GridViewColumn column = new GridViewColumn();
                    column.DisplayMemberBinding = new Binding(dc.ColumnName);
                    column.Header = dc.ColumnName;
                    mainContentWindow._listView_room.SelectionChanged += _listView_room_SelectionChanged;
                    mainContentWindow._gridView_room.Columns.Add(column);                   
                }));                
            }
        }

        public void ChangeRoomSend(int iUserId, string sTrennzeichen, string sBeschreibung, string sPicURL, bool isPrivate, string sRoomName)
        {
            AddRoomSend(iUserId, sTrennzeichen, sBeschreibung, sPicURL, isPrivate, sRoomName, IdRoomToChange);
            IsChanged = true;
        }

        private void _listView_room_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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

        public void ClearAllTxtFields()
        {
            mainContentWindow.txt_name_room.Text = "";
            mainContentWindow.txt_beschreibung_room.Text = "";
            mainContentWindow.b_isPrivate_room.IsChecked = false;
            mainContentWindow.txt_url_pic_room.Text = "";
        }

        public void AddRoomClick() {
            mainContentWindow.gb_roomInfos.Visibility = Visibility.Visible;
            mainContentWindow.btn_saveRoom.Visibility = Visibility.Visible;
            mainContentWindow.btn_cancelRoom.Visibility = Visibility.Visible;
            mainContentWindow.btn_saveChangeRoom.Visibility = Visibility.Hidden;
            mainContentWindow.btn_deleteRoom.Visibility = Visibility.Hidden;
            ClearAllTxtFields();
        }

        public void DeleteRoom()
        {
            string sDeleteRoom = "";
            string deleteRoomTrennzeichen = "|";

           
            foreach (string tmp in keyDelete)
            {
                //sDeleteRoom += deleteRoomTrennzeichen + tmp;
                TCPClient.sendMessage("#213;" + tmp, true);
                Thread.Sleep(500);
            }
            keyDelete.Clear();
        }

        public void DeleteRoomReceive(List<string> messageServer)
        {
            if (messageServer[1] == "1")
            {
                MessageBox.Show("Der Raum wurde erfolgreich gelöscht!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            else if (messageServer[1] == "2")
            {
                MessageBox.Show("Der Raum konnte nicht gelöscht werden!", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            else
            {
                MessageBox.Show("Unbekannter Protokolfehler!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
