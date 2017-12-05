﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Network;
using Xamarin.Forms;

namespace tbfApp
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            // Positioning of the ActivityIndicator in Center
            activityIndicator.WidthRequest = 80;
            activityIndicator.HeightRequest = 80;
            Constraint centerX = Constraint.RelativeToParent(parent => (parent.Width / 2) - (activityIndicator.Width / 2));
            Constraint centerY = Constraint.RelativeToParent(parent => (parent.Height / 2) - (activityIndicator.Height / 2));
            relativeLayout.Children.Add(activityIndicator, centerX, centerY);
        }

        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            activityIndicator.IsVisible = true;
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
                //await Task.Run(() => { App.Communicate("#102;" + usernameEntry.Text + ";" + passwordEntry.Text, this); });    //try to wait for answer and play the activityindicator but solved it in SimpleNetwork Client
            }

            //Application.Current.Properties["IsUserLoggedIn"] = true;
            App.LogInSwitch();

            buttonLoginin.IsEnabled = true;
            activityIndicator.IsVisible = false;
        }

        async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            activityIndicator.IsVisible = true;
            buttonSignUp.IsEnabled = false;

            //Greatings from Server
            //await App.Communicate("#001;~", this);

            App.NavigateToSignUp();

            buttonSignUp.IsEnabled = true;
            activityIndicator.IsVisible = false;
        }

        async void OnStandardSettingsButtonClicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Standardeinstellungen ", "Wollen Sie die Standardeinstellungen wieder herstellen?", "Ja", "Nein");
            if (answer)
            {
                SettingsPage settingsPage = new SettingsPage();
                settingsPage.StandardSettingsClicked(new object(), new EventArgs());
            }
        }

        private void ServerAnswer(string protocol)
        {
            //DisplayAlert("Servermessage", protocol, "OK");                    //Output of the Server Answer

            List<string> list = new List<string>();
            list = protocol.Split(new char[] { ';' } ).ToList();

            if (list.ElementAt(0).Equals("#103"))
            {
                switch (list.ElementAt(1))
                {
                    case "1":
                        int iD;
                        int.TryParse(list.ElementAt(2), out iD);
                        App.SetUserID(iD);
                        App.SetUsername(usernameEntry.Text);
                        Application.Current.Properties["IsUserLoggedIn"] = true;
                        //App.LogInSwitch();
                        DisplayAlert("Login Erfolgreich!", "Hallo " + usernameEntry.Text + "!", "OK");
                        break;
                    case "2":
                        //messageLabel.Text = "Login fehlgeschlagen, falsches Passwort oder falscher Benutzername!";
                        DisplayAlert("Login Fehlgeschlagen", "falsches Passwort oder falscher Benutzername!", "OK");
                        passwordEntry.Text = string.Empty;
                        usernameEntry.Text = string.Empty;
                        break;
                    case "3":
                        //messageLabel.Text = "Login fehlgeschlagen, Serverproblem!";
                        DisplayAlert("Login Fehlgeschlagen", "Serverproblem!", "OK");
                        break;
                    default:
                        //messageLabel.Text = "Kommunikationsproblem, Undefinierte Antwort vom Server 1!";
                        DisplayAlert("Login Fehlgeschlagen", "Kommunikationsproblem, Undefinierte Antwort vom Server 1!", "OK");
                        break;
                }

                /*
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
                */
            }
            else
            {
                //messageLabel.Text = "Kommunikationsproblem, Undefinierte Antwort vom Server 2!";
                DisplayAlert("Login Fehlgeschlagen", "Kommunikationsproblem, Undefinierte Antwort vom Server 2!", "OK");
            }
            App.endpointConnection.closeConnection();
        }
    }
}
