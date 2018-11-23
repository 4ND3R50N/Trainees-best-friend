using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MahApps.Metro.Controls;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows;
using WhiteCode.Network;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using tbfContentManager.Classes;

namespace tbfContentManager
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        SimpleNetwork_Client TCPClient;
        MainContentWindow mainContentWindow = null;
        //Buffer of txt boxes
        public string sUserBuffer = "8000";
        int Bufferlength = 18000;
        string domain = "tbf.spdns.de";        //194.55.12.202
        public string sTrennzeichen = ";";

        public MainWindow()
        {
            InitializeComponent();

            //IPAddress.Parse(ipAddress)
            TCPClient = new SimpleNetwork_Client(Server_response, Bufferlength, "", domain,
                                                13001, AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Server_response(string message) {
            List<string> lServerData = new List<string>();
            lServerData = message.Split(';').ToList();
            //MessageBox.Show(message);

            switch (lServerData[0].ToString())
            {
                case "#103":
                    LoginManager.LoginReceive(lServerData, this);
                    break;
                case "#105":
                    SignupManager.SignUp_Receive(lServerData, this);                    
                    break;

                default :
                    break;
            }
        }

        private void BtnLogin_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            LoginManager.Click_LogIn(ref TCPClient, this);
        }

        private void TxtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnLogin_Click(null, null);
            }
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        [STAThread]
        public void StartMainWindow(object sUserID)
        {
            int iUserID = Convert.ToInt32(sUserID);
            txtUser.Dispatcher.BeginInvoke((Action)(() => mainContentWindow = new MainContentWindow(ref TCPClient, sUserBuffer, iUserID)));
            txtUser.Dispatcher.BeginInvoke((Action)(() => mainContentWindow.Show()));
            this.Dispatcher.BeginInvoke((Action)(() => this.Hide()));
        }

        private void Btn_SignUp_SignUp_Click(object sender, RoutedEventArgs e)
        {
            SignupManager.SignUp_Btn_Click(ref TCPClient, this);         
        }

        private void Btn_SignUp_SignUp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnLogin_Click(null, null);
            }
        }
    }
}
