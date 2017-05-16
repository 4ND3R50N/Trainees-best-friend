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
        simpleNetwork_Client TCPClient;
        MainContentWindow mainContentWindow;

        public RoomManager(ref simpleNetwork_Client TCPClient, MainContentWindow mainContentWindow)
        {
            this.TCPClient = TCPClient;
            this.TCPClient.changeProtocolFunction(server_response_roomManager);
            this.mainContentWindow = mainContentWindow;
        }
        
        private void server_response_roomManager(string message)
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
                //MessageBox.Show("#203;" + iUserId + sTrennzeichen + txt_name_room.Text + sTrennzeichen + txt_beschreibung_room.Text + sTrennzeichen + i_isPrivate_room + sTrennzeichen + txt_url_pic_room.Text + sTrennzeichen);
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
            DataTable roomTable = new DataTable();
            roomTable = new DataTable();
            roomTable.Columns.Add("Räume");

            for (int i = 2; i < messageList.Count; i++)
            {
                List<string> roomData = new List<string>();
                roomData = messageList.ElementAt(i).Split('|').ToList();

                roomTable.Rows.Add(roomData.ElementAt(1));
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

            foreach (var colum in dt.Columns)
            {
                mainContentWindow._gridView_room.Dispatcher.BeginInvoke((Action)(() => {
                    DataColumn dc = (DataColumn)colum;
                    GridViewColumn column = new GridViewColumn();
                    column.DisplayMemberBinding = new Binding(dc.ColumnName);

                    column.Header = dc.ColumnName;
                    mainContentWindow._gridView_room.Columns.Add(column);
                }));
            }
        }

    }
}
