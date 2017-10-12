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
        SimpleNetwork_Client TCPClient;

        RoomManager roomManager;
        WorkoutManager workoutManager;

        public MainContentWindow(ref SimpleNetwork_Client TCPClient, string sUserName, int iUserID)
        {
            InitializeComponent();

            this.sUserName = sUserName;
            this.iUserId = iUserID;
            this.TCPClient = TCPClient;

            roomManager = new RoomManager(ref TCPClient, this, iUserID);
            workoutManager = new WorkoutManager(ref TCPClient, this, roomManager);


            lblWelcomeMessage.Content = "Willkommen " + sUserName;
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

        // ----------------------------------------------- Raum Manager -------------------------------------------------------- //

        private void Btn_addRoom_Click(object sender, RoutedEventArgs e)
        {
            roomManager.AddRoomClick();
        }

        private void Btn_saveRoom_Click(object sender, RoutedEventArgs e)
        {
            roomManager.AddRoomSend(iUserId, sTrennzeichen, txt_beschreibung_room.Text, txt_url_pic_room.Text,
                (bool) b_isPrivate_room.IsChecked, txt_name_room.Text, "0");
            Thread.Sleep(2000);
            roomManager.GetAllRoomSend();
        }

        private void B_url_pic_room_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".txt";
            //dlg.Filter = "Bildformat(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF";
            dlg.Filter = "Bildformat(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*";
            
            //spater fuer video
            /*
             dlg.Filter = "Videoformat (...) |*.dat; *.wmv; *.3g2; *.3gp; *.3gp2; *.3gpp; *.amv; *.asf;  *.avi; *.bin; *.cue; *.divx; *.dv; *.flv; *.gxf; *.iso;" + 
             "*.m1v; *.m2v; *.m2t; *.m2ts; *.m4v; *.mkv; *.mov; *.mp2; *.mp2v; *.mp4; *.mp4v; *.mpa; *.mpe; *.mpeg; *.mpeg1; *.mpeg2; *.mpeg4;" + 
             "*.mpg; *.mpv2; *.mts; *.nsv; *.nuv; *.ogg; *.ogm; *.ogv; *.ogx; *.ps; *.rec; *.rm; *.rmvb; *.tod; *.ts; *.tts; *.vob; *.vro; *.webm";
             */

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;

                //Upload File via FTP
                //string source = @"FilePath and FileName of Local File to Upload";
                //string destination = @"SFTP Server File Destination Folder";
                //string host = "SFTP Host";
                //string username = "User Name";
                //string password = "password";
                //int port = 22;  //Port 22 is defaulted for SFTP upload
                try
                {
                    Upload.UploadSFTPFile(this, "tbf.spdns.de", "contentmanager", "TBF123", filename, "./", 13002);

                    txt_url_pic_room.Text = filename;
                }
                catch (Exception)
                {
                    MessageBox.Show("Fehler beim Hochladen der Datei!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
            //System.Drawing.Image img = System.Drawing.Image.FromFile(@ + filename);
            //MessageBox.Show("Width: " + .Width + ", Height: " + img.Height);

        }

        private void Btn_cancel_room_Click(object sender, RoutedEventArgs e)
        {
            roomManager.ClearAllTxtFields();
        }

        private void Btn_Delete_room_Click(object sender, RoutedEventArgs e)
        {
            roomManager.DeleteRoom();
            Thread.Sleep(2000);
            roomManager.GetAllRoomSend();
        }

        private void TiRoomManager_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TCPClient.changeProtocolFunction(roomManager.Server_response_roomManager);

            //gb_workoutInfos.Visibility = Visibility.Hidden;
            roomManager.GetAllRoomSend();
            Thread.Sleep(100);

        }

        private void btn_saveChangeRoom_Click(object sender, RoutedEventArgs e)
        {
            roomManager.ChangeRoomSend(iUserId, sTrennzeichen, txt_beschreibung_room.Text, txt_url_pic_room.Text, (bool)b_isPrivate_room.IsChecked, txt_name_room.Text);
        }

        // --------------------------------------------------------------- Workout Manager ---------------------------------------------------- //
        
        private void btn_addWorkout_Click(object sender, RoutedEventArgs e)
        {
            workoutManager.AddWorkoutClick();
        }

        private void btn_saveWorkout_Click(object sender, RoutedEventArgs e)
        {
            string roomId = "111";
            workoutManager.AddWorkoutSend(iUserId, sTrennzeichen, txt_beschreibung_workout.Text, txt_url_pic_workout.Text, txt_name_workout.Text, "0", roomId);

        }

        private void B_url_pic_workout_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Bildformat (*.JPG; *.PNG; )|*.JPG, *.PNG";

            //spater fuer video
            
             //dlg.Filter = "Videoformat (...) |*.dat; *.wmv; *.3g2; *.3gp; *.3gp2; *.3gpp; *.amv; *.asf;  *.avi; *.bin; *.cue; *.divx; *.dv; *.flv; *.gxf; *.iso;" + 
             //"*.m1v; *.m2v; *.m2t; *.m2ts; *.m4v; *.mkv; *.mov; *.mp2; *.mp2v; *.mp4; *.mp4v; *.mpa; *.mpe; *.mpeg; *.mpeg1; *.mpeg2; *.mpeg4;" + 
             //"*.mpg; *.mpv2; *.mts; *.nsv; *.nuv; *.ogg; *.ogm; *.ogv; *.ogx; *.ps; *.rec; *.rm; *.rmvb; *.tod; *.ts; *.tts; *.vob; *.vro; *.webm";
             

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                txt_url_pic_room.Text = filename;
            }
            //System.Drawing.Image img = System.Drawing.Image.FromFile(@ + filename);
            //MessageBox.Show("Width: " + .Width + ", Height: " + img.Height);

        }

        private void Btn_cancel_workout_Click(object sender, RoutedEventArgs e)
        {
            workoutManager.ClearAllTxtFields();
        }

        private void Btn_Delete_workout_Click(object sender, RoutedEventArgs e)
        {
            workoutManager.DeleteWorkout();
        }

        private void TiWorkoutManager_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TCPClient.changeProtocolFunction(roomManager.Server_response_roomManager);
            roomManager.GetAllRoomSend();
            Thread.Sleep(400);

            TCPClient.changeProtocolFunction(workoutManager.Server_response_workoutManager);
            gb_roomInfos.Visibility = Visibility.Hidden;
            workoutManager.ShowAllRooms();
        }

        //private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    MessageBox.Show("");
        //}

        //private void workout_grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    MessageBox.Show("mache nichts");
        //}
        
    }
 }
