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
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using tbfContentManager.Classes;
using WhiteCode.Network;

namespace tbfContentManager
{
    public partial class MainContentWindow
    {
        string sUserName;
        int iUserId;
        string sTrennzeichen = ";";
        DataTable table;

        RoomManager roomManager;

        public MainContentWindow(ref SimpleNetwork_Client TCPClient, string sUserName, int iUserID)
        {
            InitializeComponent();
            
            this.sUserName = sUserName;
            this.iUserId = iUserID;
            //CHangeprotocolfunction in den konstruktor verschoben, da dieser einmalig für den formwechsel gemacht werden muss
            //Es wäre natürlich ordentlicher, wenn du für jeden TAB eine Server_response funktion erstellen würdest. 
            //IN dem fall müsstest du jedesmal wenn man den tab wechselt, die changeProtocolFunction() erneut ausführen :)

            roomManager = new RoomManager(ref TCPClient, this);

            lblWelcomeMessage.Content = "Willkommen " + sUserName + " Userid " + iUserId;

            table = new DataTable();
            table.Columns.Add("Workout");
            table.Columns.Add("Raum");
            table.AcceptChanges();
            table.Rows.Add("Bauchmuskeln", "asdfasdf");
            table.Rows.Add("Testworkout", "Raume1");
            //exampleTable.Rows.Add("a2", "b2");
            table.AcceptChanges();

            LoadTable_Workout(table);
        }

        
        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            MainWindow newLogin = new MainWindow();
            newLogin.Show();
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Btn_addRoom_Click(object sender, RoutedEventArgs e)
        {
            gb_roomInfos.Visibility = Visibility;
        }

        private void Btn_saveRoom_Click(object sender, RoutedEventArgs e)
        {
            roomManager.AddRoomSend(iUserId, sTrennzeichen, txt_beschreibung_room.Text, 
                txt_url_pic_room.Text, (bool)b_isPrivate_room.IsChecked, txt_name_room.Text);
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
            _listView.Dispatcher.BeginInvoke((Action)(() => _listView.DataContext = dt));
                       
            _gridView.Dispatcher.BeginInvoke((Action)(() => _gridView.Columns.Clear()));

            Binding bind = new Binding();
            
            _listView.Dispatcher.BeginInvoke((Action)(() => _listView.SetBinding(ListView.ItemsSourceProperty, bind)));

            foreach (var colum in dt.Columns)
            {
                DataColumn dc = (DataColumn)colum;
                GridViewColumn column = new GridViewColumn();
                column.DisplayMemberBinding = new Binding(dc.ColumnName);
             
                column.Header = dc.ColumnName;
                _gridView.Dispatcher.BeginInvoke((Action)(() => _gridView.Columns.Add(column)));
            }
        }

        private void TiRoomManager_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            roomManager.GetAllRoomSend();
        }
        
        private void TiRoomManager_Loaded(object sender, RoutedEventArgs e)
        {
            //RoomManager.GetAllRoomSend(ref TCPClient);
        }
    }
}
