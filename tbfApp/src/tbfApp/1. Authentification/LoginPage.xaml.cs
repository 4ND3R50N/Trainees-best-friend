using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Network;
using Sockets.Plugin.Abstractions;
using Xamarin.Forms;

namespace tbfApp
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            activityIndicator.IsRunning = true;
            buttonLoginin.IsEnabled = false;

            if (usernameEntry.Text == null)
            {
                await DisplayAlert("Eingabefehler", "Benutzername eingeben!", "OK");
            }
            else if (passwordEntry.Text == null)
            {
                await DisplayAlert("Eingabefehler", "Passwort eingeben!", "OK");
            }
            else
            {
                //Login Request
                App.endpointConnection.SetProtocolFunction(this.ServerAnswer);
                await App.Communicate("#102;" + usernameEntry.Text + ";" + passwordEntry.Text, this);
            }

            //Application.Current.Properties["IsUserLoggedIn"] = true;
            App.LogInSwitch();

            buttonLoginin.IsEnabled = true;
            activityIndicator.IsRunning = false;
        }

        async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            activityIndicator.IsRunning = true;
            buttonSignUp.IsEnabled = false;

            //Greatings from Server
            //await App.Communicate("#001;~", this);

            App.NavigateToSignUp();

            buttonSignUp.IsEnabled = true;
            activityIndicator.IsRunning = false;
        }

        private void ServerAnswer(string protocol)
        {
            DisplayAlert("Servermessage", protocol, "OK");

            List<string> list = new List<string>();
            list = protocol.Split(new char[] { ';' } ).ToList();

            if (list.ElementAt(0).Equals("#103"))
            {
                if (list.ElementAt(1).Equals("1"))
                {
                    int iD;
                    int.TryParse(list.ElementAt(2), out iD);
                    App.SetUserID(iD);
                    App.SetUsername(usernameEntry.Text);
                    Application.Current.Properties["IsUserLoggedIn"] = true;
                    //App.LogInSwitch();
                    DisplayAlert("Login Erfolgreich!", "Hallo " + usernameEntry.Text + "!", "OK");
                }
                else if(list.ElementAt(1).Equals("2"))
                {
                    messageLabel.Text = "Login fehlgeschlagen, falsches Passwort oder falscher Benutzername!";
                    passwordEntry.Text = string.Empty;
                    usernameEntry.Text = string.Empty;
                }
                else if(list.ElementAt(1).Equals("3"))
                {
                    messageLabel.Text = "Login fehlgeschlagen, Serverproblem!";
                }
                else
                {
                    messageLabel.Text = "Kommunikationsproblem, Undefinierte Antwort vom Server 1!";
                }
            }
            else
            {
                messageLabel.Text = "Kommunikationsproblem, Undefinierte Antwort vom Server 2!";
            }
            App.endpointConnection.closeConnection();
        }
    }
}
