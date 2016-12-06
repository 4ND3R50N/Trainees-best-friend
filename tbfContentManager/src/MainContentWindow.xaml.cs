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
            List<List<string>> lServerData = new List<List<string>>();
            tmp = message.Split(';').ToList();
            string prot = message.Split(';')[0];
            string counter = message.Split(';')[1];

            //MessageBox.Show(message);

            switch (prot)
            {
                case "#202":
                    //tel_202_Room_Data(tmp, lServerData);
                    for (int i = 2; i <= Convert.ToInt32(counter); i++)
                    {
                        lServerData.Add(tmp[i].Split('|').ToList());
                    }
                    for (int j = 0; j < lServerData.Count; j++)
                    {
                            int room_ID = Convert.ToInt32(lServerData[j][0]);
                            MessageBox.Show(Convert.ToString(room_ID));
                    }
                    break;
            default:
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

        #region Support classes
        public class lRoomNameEntry
        {
            public string Name { get; set; }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //RoomManger Room laden
            TCPClient.changeProtocolFunction(server_response);
            TCPClient.sendMessage("#201", true);
        }


        #endregion

        //private void tiRoomManager_MouseUp(object sender, MouseButtonEventArgs e)
        //{
        //    if (!TCPClient.connect())
        //    {
        //        MessageBox.Show("test_Loaded");
        //        return;
        //    }
        //    TCPClient.sendMessage("#201", true);


        //    var gridView = new GridView();

        //    gridView.Columns.Add(new GridViewColumn
        //    {
        //        Header = "Name",
        //        DisplayMemberBinding = new Binding("Name")
        //    });

        //    // Populate list
        //    this.lvRoomList.Items.Add(new lRoomNameEntry { Name = "David" });
        //}

        //private void btnRoomManager_MouseUp(object sender, MouseButtonEventArgs e)
        //{
        //    if (!TCPClient.connect())
        //    {
        //        MessageBox.Show("test_Loaded");
        //        return;
        //    }
        //    TCPClient.sendMessage("#201", true);


        //    var gridView = new GridView();

        //    gridView.Columns.Add(new GridViewColumn
        //    {
        //        Header = "Name",
        //        DisplayMemberBinding = new Binding("Name")
        //    });

        //    // Populate list
        //    this.lvRoomList.Items.Add(new lRoomNameEntry { Name = "David" });
        //}

    }
}
