using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using WhiteCode.Network;

namespace tbfContentManager.Classes
{
    class LoginManager
    {

        public static void LoginReceive(List<string> lServerData, MainWindow mainWindow) {
            if (lServerData[1] == "1")
            {
                mainWindow.txtUser.Dispatcher.BeginInvoke((Action)(() => mainWindow.sUserBuffer = mainWindow.txtUser.Text));
                ParameterizedThreadStart pts = new ParameterizedThreadStart(mainWindow.StartMainWindow);
                Thread thread = new Thread(pts);
                thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
                thread.Start(lServerData[2]);

            }
            if (lServerData[1] == "2")
            {
                MessageBox.Show("Benutzername oder Password ist falsch!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (lServerData[1] == "3")
            {
                MessageBox.Show("Der Server hat einen internen Fehler! Bitte kontaktieren Sie einen Administrator!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            mainWindow.txtUser.Dispatcher.BeginInvoke((Action)(() => mainWindow.txtUser.Text = ""));
            mainWindow.txtPassword.Dispatcher.BeginInvoke((Action)(() => mainWindow.txtPassword.Password = ""));
        }


        public static void Click_LogIn(ref SimpleNetwork_Client TCPClient, MainWindow mainWindow)
        {
            mainWindow.btnLogin.IsEnabled = false;

            //-------------------- ohne login ---------------------//
            //MainContentWindow w = new MainContentWindow(ref TCPClient, "test", 0);
            //w.Show();
            //return;

            if (mainWindow.txtPassword.Password.Length > 3 && mainWindow.txtUser.Text.Length > 3)
            {
                if (!TCPClient.connect())
                {
                    mainWindow.btnLogin.IsEnabled = true;
                    MessageBox.Show("Verbindung mit dem Server konnte nicht aufgebaut werden!");
                    return;
                }
                TCPClient.sendMessage("#102;" + mainWindow.txtUser.Text + ";" + mainWindow.txtPassword.Password, true);
            }
            else
            {
                mainWindow.btnLogin.IsEnabled = true;
                MessageBox.Show("Benutzername oder Passwort ist zu kurz! (mindestens 4 Zeichen)", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
