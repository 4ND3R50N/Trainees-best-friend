using System;
using System.Collections.Generic;
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
using WhiteCode.Network;

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
            this.iUserId = iUserId;
        }

        private void server_response(string message)
        {

            List<string> tmp = new List<string>();
           
            tmp = message.Split(';').ToList();
            string prot = message.Split(';')[0];
            string counter = message.Split(';')[1];
            
            //MessageBox.Show(message);

            switch (prot)
            {
                case "#202":
                    //tel_202_Room_Data(tmp, lServerData);
                    List<List<string>> lServerData = new List<List<string>>();
                    string[] room_names = new string[Convert.ToInt32(counter)];
                    for (int i = 2; i <= Convert.ToInt32(counter); i++)
                    {
                        lServerData.Add(tmp[i].Split('|').ToList());
                    }
                    for (int j = 0; j < (lServerData.Count); j++)
                    {
                            int room_ID = Convert.ToInt32(lServerData[j][0]);
                            room_names[j] = lServerData[j][1];
                        MessageBox.Show(room_names[j]);
                        //string room_description = lServerData[j][2];
                        //bool is_priavte = Convert.ToBoolean(lServerData[j][3]);
                        //string room_icon_url = lServerData[j][4];
                    }
                    for (int d = 0; d < (room_names.Length); d++) {
                        create_RoomList_item(room_names[d]);
                    }

                    break;
            default:
                    break;
                case "#204":
                    //MessageBox.Show(message);
                    MessageBox.Show(tmp[1]);
                    if (Convert.ToInt32(tmp[1]) == 1) {
                        MessageBox.Show("Der Raum " + txt_name_room.Text +" wurde erfolgreich angelegt!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    if (tmp[1] == "2")
                    {
                        MessageBox.Show("Der Raumname " + txt_name_room.Text + " existiert bereits!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    if (tmp[1] == "3")
                    {
                        MessageBox.Show("Der Server hat einen internen Fehler! Bitte kontaktieren Sie einen Administrator!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            MainWindow newLogin = new MainWindow();
            newLogin.Show();
        }

        private void tel_202_Room_Data(List<string> tmp, List<List<string>> lServerData) {
           
            for (int i = 2; i < (Convert.ToInt32(tmp[1]) + 2); i++)
            {
                lServerData.Add(tmp[i].Split('|').ToList());
            }
            for (int j = 0; j < lServerData.Count; j++) {
                for (int i = 0; i < lServerData[j].Count; i++)
                {
                    int room_ID = Convert.ToInt32(lServerData[j][i]);
                }
            }
        }


        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btn_add_room_Click(object sender, RoutedEventArgs e)
        {
            //;
            //{ Int: UserID}
            //{ String: RoomName}

            //TCPClient.sendMessage("#203;" + txtUser.Text + ";" + txtPassword.Password, true);
        }

        public void create_RoomList_item(string room_name) {
            // Populate list
            //MessageBox.Show(room_name);
            //this.lvRoomList.Items.Add(new lRoomNameEntry { Name = "David" });
            //this.lvRoomList.Items.Add(new lRoomNameEntry { Name = room_name });
        }


        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //RoomManger Room laden
            TCPClient.changeProtocolFunction(server_response);
            TCPClient.sendMessage("#201", true);

            var gridView = new GridView();
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Name",
                DisplayMemberBinding = new Binding("Name")
            });
        }

     
        private void btn_addRoom_Click(object sender, RoutedEventArgs e)
        {
            gb_roomInfos.Visibility = Visibility;
        }

        private void btn_saveRoom_Click(object sender, RoutedEventArgs e)
        {
            int i_isPrivate_room = 0;
            if(b_isPrivate_room.IsChecked == true) {
                i_isPrivate_room = 1;
            }else{
                i_isPrivate_room = 0;
            }
            if (txt_name_room.Text.Length > 0) {
                // WICHTIG FUER SPAETER!!! //
                /*
                    Bild muss vorher auf DB geschickt 
                    der schickt dann URL zurueck, dass ist dann die txt_url_room 
                 */

                //MessageBox.Show("#203;" + iUserId + sTrennzeichen + txt_name_room.Text + sTrennzeichen + txt_beschreibung_room.Text + sTrennzeichen + i_isPrivate_room + sTrennzeichen + txt_url_pic_room.Text + sTrennzeichen);
                TCPClient.sendMessage("#203;" + iUserId + sTrennzeichen + txt_name_room.Text + sTrennzeichen 
                    + txt_beschreibung_room.Text + sTrennzeichen + i_isPrivate_room + sTrennzeichen + txt_url_pic_room.Text + sTrennzeichen, true);
            }
        }

        private void b_url_pic_room_Click(object sender, RoutedEventArgs e)
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

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            txt_name_room.Text = "";
            txt_beschreibung_room.Text = "";
            b_isPrivate_room.IsChecked = false;
            txt_url_pic_room.Text = "";
        }

        //private void tiRoomManager_MouseUp(object sender, MouseButtonEventArgs e)
        //{
        //    //    var gridView = new GridView();

        //    gridView.Columns.Add(new GridViewColumn
        //    {
        //        Header = "Name",
        //        DisplayMemberBinding = new Binding("Name")
        //    });

        //    // Populate list
        //    this.lvRoomList.Items.Add(new lRoomNameEntry { Name = "David" });
        //

    }
}
