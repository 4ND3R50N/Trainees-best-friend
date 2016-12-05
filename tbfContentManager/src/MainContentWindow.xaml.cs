using System;
using System.Collections.Generic;
using System.Linq;
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
       
        public MainContentWindow(simpleNetwork_Client TCPClient, string sUserName, int iUserID)
        {

            InitializeComponent();  
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            MainWindow newLogin = new MainWindow();
            newLogin.Show();
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

        private void tiRoomManager_Loaded(object sender, RoutedEventArgs e)
        {
            var gridView = new GridView();

            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Name",
                DisplayMemberBinding = new Binding("Name")
            });

            // Populate list
            this.lvRoomList.Items.Add(new lRoomNameEntry {Name = "David" });
        }
        #region Support classes
        public class lRoomNameEntry
        {
            public string Name { get; set; }
        }
        #endregion


    }
}
