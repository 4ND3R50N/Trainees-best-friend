using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using tbfContentManager.Classes;
using WhiteCode.Network;
//using tbfContentManager.Classes;

namespace tbfContentManager
{
    public partial class MainContentWindow
    {
        simpleNetwork_Client TCPClient;
        string sUserName;
        int iUserId;
        string sTrennzeichen = ";";

       

        public MainContentWindow(ref simpleNetwork_Client TCPClient, string sUserName, int iUserID)
        {
            InitializeComponent();

            this.TCPClient = TCPClient;
            this.sUserName = sUserName;
            this.iUserId = iUserID;

            lblWelcomeMessage.Content = "Willkommen " + sUserName;

           DataTable exampleTable = new DataTable();
            exampleTable.Columns.Add("Workout");
            exampleTable.Columns.Add("Raum");
            exampleTable.AcceptChanges();
            exampleTable.Rows.Add("Bauchmuskeln", "asdfasdf");
            exampleTable.Rows.Add("Testworkout", "Raume1");
            //exampleTable.Rows.Add("a2", "b2");
            exampleTable.AcceptChanges();

            LoadTable_Workout(exampleTable);
        }

        private void Server_response(string message)
        {

            List<string> messageList = new List<string>();

            messageList = message.Split(';').ToList();
            string prot = message.Split(';')[0];

            //MessageBox.Show(message);

            switch (prot)
            {
                case "#202":
                    int roomAmount = messageList.Count - 2;

                    DataTable roomTable = new DataTable();
                    roomTable.Columns.Add("Räume");

                    for (int i = 2; i <= roomAmount + 2; i++)
                    {
                        List<string> roomData = new List<string>();
                        roomData = messageList.ElementAt(i).Split('|').ToList();

                        roomTable.Rows.Add(roomData.ElementAt(1));
                    }

                    roomTable.AcceptChanges();
                    LoadTable_Room(roomTable);

                    /*
                    //MessageBox.Show(message);
                    //Tel_202_Room_Data(tmp, lServerData);

                    //string counter = message.Split(';')[1];

                    int counter = tmp.Count() - 1;
                    List<List<string>> lServerData = new List<List<string>>();

                    //string[] room_names = new string[Convert.ToInt32(counter)];

                    string[] room_names = new string[counter-1];
                    //List<string> room_names = new List<string>();

                    //for (int i = 2; i <= Convert.ToInt32(counter); i++)

                    for (int i = 2; i <= counter; i++)
                    {
                        lServerData.Add(tmp[i].Split('|').ToList());
                    }
                    for (int j = 0; j < (lServerData.Count); j++)
                    {
                        int room_ID = Convert.ToInt32(lServerData[j][0]);
                        room_names[j] = lServerData[j][1];
                        MessageBox.Show(room_names[j]);
                        //_listView_room.Items.Add(new RoomItem { Raeume = room_names[j] });
                        //string room_description = lServerData[j][2];
                        //bool is_priavte = Convert.ToBoolean(lServerData[j][3]);
                        //string room_icon_url = lServerData[j][4];
                    }
                    //string[] data = { "hallo", "avelina", "alles", "super" };
                    //foreach (string t in data)
                    //{
                    //    myListBox.Items.Add(t);
                    //    //myListView.Items.Add(t);
                    //}
                    //foreach (string t in room_names)
                    //{
                    //    myListBox.Items.Add(t);
                    //    myListView.Items.Add(t);
                    //}
                    //for (int k = 0; k < room_names.Length; k++)
                    //{
                    //    //_listView_room.Items.Add(new RoomItem { Raeume = room_names[k] });
                    //}

                    _listView_room.ItemsSource = room_names;
                    DataTable exampleTable = new DataTable();
                    exampleTable.Columns.Add("Räume");
                    exampleTable.AcceptChanges();
                    for (int d = 0; d < (room_names.Length); d++)
                    {
                        //create_RoomList_item(room_names[d]);
                        exampleTable.Rows.Add(room_names[d]);
                        exampleTable.AcceptChanges();
                    }
                    exampleTable.AcceptChanges();
                    LoadTable_Room(exampleTable);
                    */
                    break;
                case "#204":
                    RoomManager.AddRoomReceive(messageList);
                    break;
                default:
                    MessageBox.Show("Server Kommunikationsproblem!");
                    break;
            }
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            MainWindow newLogin = new MainWindow();
            newLogin.Show();
        }

        //private void Tel_202_Room_Data(List<string> tmp, List<List<string>> lServerData)
        //{
        //    for (int i = 2; i < (Convert.ToInt32(tmp[1]) + 2); i++)
        //    {
        //        lServerData.Add(tmp[i].Split('|').ToList());
        //    }
        //    for (int j = 0; j < lServerData.Count; j++)
        //    {
        //        for (int i = 0; i < lServerData[j].Count; i++)
        //        {
        //            int room_ID = Convert.ToInt32(lServerData[j][i]);
        //        }
        //    }
        //}


        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        //private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        //{
        //    //RoomManger Room laden
        //    TCPClient.changeProtocolFunction(Server_response);
        //    TCPClient.sendMessage("#201", true);

        //    var gridView = new GridView();
        //    gridView.Columns.Add(new GridViewColumn
        //    {
        //        Header = "Name",
        //        DisplayMemberBinding = new Binding("Name")
        //    });
        //}


        private void Btn_addRoom_Click(object sender, RoutedEventArgs e)
        {
            gb_roomInfos.Visibility = Visibility;
        }

        private void Btn_saveRoom_Click(object sender, RoutedEventArgs e)
        {
            //RoomManager.AddRoomSend(ref TCPClient, iUserId, sTrennzeichen, txt_beschreibung_room.Text, txt_url_pic_room.Text,(bool) b_isPrivate_room.IsChecked, txt_name_room.Text);
        }

        private void B_url_pic_room_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".txt";
            //dlg.Filter = "Text documents (.txt)|*.txt";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                txt_url_pic_room.Text = filename;
            }
        }

        private void Btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            txt_name_room.Text = "";
            txt_beschreibung_room.Text = "";
            b_isPrivate_room.IsChecked = false;
            txt_url_pic_room.Text = "";
        }

        private void LoadTable_Workout(DataTable dt)
        {
            _listView.DataContext = dt;

            _gridView.Columns.Clear();

            Binding bind = new Binding();
            _listView.SetBinding(ListView.ItemsSourceProperty, bind);

            foreach (var colum in dt.Columns)
            {
                DataColumn dc = (DataColumn)colum;
                GridViewColumn column = new GridViewColumn();
                column.DisplayMemberBinding = new Binding(dc.ColumnName);
             
                column.Header = dc.ColumnName;
                _gridView.Columns.Add(column);
            }
        }
/*
        private void tiRoomManager_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //RoomManger Room laden
            TCPClient.changeProtocolFunction(Server_response);
            TCPClient.sendMessage("#201", true);

            //    //RoomManager.GetAllRoomSend(ref TCPClient);
        }
        */
        private void LoadTable_Room(DataTable dt)
        {
            _listView_room.DataContext = dt;

            _gridView_room.Columns.Clear();

            Binding bind = new Binding();
            _listView_room.SetBinding(ListView.ItemsSourceProperty, bind);

            foreach (var colum in dt.Columns)
            {
                DataColumn dc = (DataColumn)colum;
                GridViewColumn column = new GridViewColumn();
                column.DisplayMemberBinding = new Binding(dc.ColumnName);

                column.Header = dc.ColumnName;
                _gridView_room.Columns.Add(column);
            }
        }

        private void tiRoomManager_Loaded(object sender, RoutedEventArgs e)
        {
            //RoomManger Room laden
            TCPClient.changeProtocolFunction(Server_response);
            TCPClient.sendMessage("#201", true);

            //RoomManager.GetAllRoomSend(ref TCPClient);
        }
    }
}
