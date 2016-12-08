using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace tbfApp
{
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
        }
        async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            activityIndicator.IsRunning = true;
            buttonSignUp.IsEnabled = false;

            if (usernameEntry.Text == null || forenameEntry.Text == null || secondnameEntry.Text == null || passwordEntry.Text == null || passwordEntry2.Text == null || emailEntry.Text == null)
            {
                //await DisplayAlert("Eingabefehler", "Eingaben nicht vollständig!", "OK");
                messageLabel.Text = "Eingaben nicht vollständig!";
            }
            else if (!passwordEntry.Text.Equals(passwordEntry2.Text))
            {
                //await DisplayAlert("Eingabefehler", "Passwörter nicht identisch!", "OK");
                messageLabel.Text = "Passwörter nicht identisch!";
                passwordEntry.Text = string.Empty;
                passwordEntry2.Text = string.Empty;
            }
            else if (!emailEntry.Text.Contains("@") || !emailEntry.Text.Contains("."))
            {
                //await DisplayAlert("Eingabefehler", "Email Adresse ungültig!", "OK");
                messageLabel.Text = "Email Adresse ungültig!";
                emailEntry.Text = string.Empty;
            }
            else
            {
                //Login Request
                App.endpointConnection.SetProtocolFunction(this.ServerAnswer);
                await App.Communicate("#104;" + usernameEntry.Text + ";" + secondnameEntry.Text + ";" + forenameEntry.Text + ";" + passwordEntry.Text + ";" + emailEntry.Text.ToLower(), this);
            }

            buttonSignUp.IsEnabled = true;
            activityIndicator.IsRunning = false;
        }

        private void ServerAnswer(string protocol)
        {
            DisplayAlert("Servermessage", protocol, "OK");

            List<string> list = new List<string>();
            //string [] test  = protocol.Split(';');
            list = protocol.Split(';').ToList();

            if (list.ElementAt(0).Equals("#105"))
            {
                if (list.ElementAt(1).Equals("1"))
                {
                    Navigation.InsertPageBefore(new MainPage(), Navigation.NavigationStack.First());
                    Navigation.PopToRootAsync();
                    DisplayAlert("Sign Up erfolgreich!","Sie können sich jetzt einloggen", "OK");
                }
                else if (list.ElementAt(1).Equals("2"))
                {
                    messageLabel.Text = "Sign Up fehlgeschlagen, Benutzername existiert bereits!";
                    usernameEntry.Text = string.Empty;
                }
                else if (list.ElementAt(1).Equals("3"))
                {
                    messageLabel.Text = "Sign Up fehlgeschlagen, Serverproblem!";
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

            /*
            if (true)
            {
                var rootPage = Navigation.NavigationStack.FirstOrDefault();
                if (rootPage != null)
                {
                    Application.Current.Properties["IsUserLoggedIn"] = true;
                    Navigation.InsertPageBefore(new MainPage(), Navigation.NavigationStack.First());
                    Navigation.PopToRootAsync();
                }
            }
            else
            {
                messageLabel.Text = "Sign up failed!";
            }
            */
        }
    }
}
