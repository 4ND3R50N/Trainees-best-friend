using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using WhiteCode.Network;


namespace tbfContentManager.Classes
{
    class SignupManager
    {
        public static string sTrennzeichen = ";";

        public static void SignUp_Receive(List<string> lServerData, MainWindow mainWindow) {
            if (lServerData[1] == "1")
            {
                MessageBox.Show("Der Account wurde erfolgreich erstellt!", "Successfull", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (lServerData[1] == "2")
            {
                mainWindow.txt_UserName_SignUp.Dispatcher.BeginInvoke((Action)(() => mainWindow.txt_UserName_SignUp.Text = ""));
                mainWindow.btn_SignUp_SignUp.Dispatcher.BeginInvoke((Action)(() => mainWindow.btn_SignUp_SignUp.IsEnabled = true));
                MessageBox.Show("Der Username existiert bereits!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);   
            }
            if (lServerData[1] == "3")
            {
                mainWindow.txt_Email_SignUp.Dispatcher.BeginInvoke((Action)(() => mainWindow.txt_Email_SignUp.Text = ""));
                mainWindow.btn_SignUp_SignUp.Dispatcher.BeginInvoke((Action)(() => mainWindow.btn_SignUp_SignUp.IsEnabled = true));
                MessageBox.Show("Die E-Mail-Adresse existiert bereits!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (lServerData[1] == "4")
            {
                mainWindow.btn_SignUp_SignUp.Dispatcher.BeginInvoke((Action)(() =>  mainWindow.btn_SignUp_SignUp.IsEnabled = true));
                MessageBox.Show("Der Server hat einen internen Fehler! Bitte kontaktieren Sie einen Administrator!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);              
            }
        }

        public static void SignUp_Btn_Click(ref SimpleNetwork_Client TCPClient, MainWindow mainWindow) {
            if (mainWindow.txt_UserName_SignUp.Text == null || mainWindow.txt_Password_SignUp.Password == null ||
                mainWindow.txt_Password_Repeat_SignUp.Password == null || mainWindow.txt_Email_SignUp.Text == null ||
                mainWindow.txt_Forname_SignUp.Text == null || mainWindow.txt_Secondname_SignUp.Text == null)
            {
                MessageBox.Show("Eingaben sind nicht vollständig!", "Eingabefehler!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (mainWindow.txt_UserName_SignUp.Text.Length < 3)
            {
                MessageBox.Show("Der Benutzername muss mindestens 3 Zeichen enthalten!", "Eingabefehler!", MessageBoxButton.OK, MessageBoxImage.Error);
                mainWindow.txt_UserName_SignUp.Text = "";
            }
            else if (mainWindow.txt_Password_SignUp.Password.Length < 3)
            {
                MessageBox.Show("Der Passwort muss mindestens 3 Zeichen enthalten!", "Eingabefehler!", MessageBoxButton.OK, MessageBoxImage.Error);
                mainWindow.txt_Password_SignUp.Password = "";
                mainWindow.txt_Password_Repeat_SignUp.Password = "";
            }
            else if (!mainWindow.txt_Password_SignUp.Password.Equals(mainWindow.txt_Password_Repeat_SignUp.Password))
            {
                MessageBox.Show("Passwörter sind nicht identisch!", "Eingabefehler!", MessageBoxButton.OK, MessageBoxImage.Error);
                mainWindow.txt_Password_SignUp.Password = "";
                mainWindow.txt_Password_Repeat_SignUp.Password = "";
            }
            else if (!mainWindow.txt_Email_SignUp.Text.Contains("@") || !mainWindow.txt_Email_SignUp.Text.Contains("."))
            {
                MessageBox.Show("E-Mail-Adresse nicht gültig!", "Eingabefehler!", MessageBoxButton.OK, MessageBoxImage.Error);
                mainWindow.txt_Email_SignUp.Text = "";
            }
            else
            {
                TCPClient.sendMessage("#104" + sTrennzeichen + mainWindow.txt_UserName_SignUp.Text + sTrennzeichen + 
                    mainWindow.txt_Secondname_SignUp.Text + sTrennzeichen + mainWindow.txt_Forname_SignUp.Text + sTrennzeichen +
                    mainWindow.txt_Password_SignUp.Password + sTrennzeichen + mainWindow.txt_Email_SignUp.Text + sTrennzeichen + "1", true);
                mainWindow.btn_SignUp_SignUp.IsEnabled = false;
            }
        }
    }
}
