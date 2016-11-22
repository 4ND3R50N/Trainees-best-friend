using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network;
using Xamarin.Forms;
using Sockets.Plugin;

namespace tbfApp
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
           await Navigation.PushAsync(new SignUpPage());
        }

        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            var user = new User
            {
                Username = usernameEntry.Text,
                Password = passwordEntry.Text
            };

            #region Network test
            SimpleNetworkClient endpointConnection = new SimpleNetworkClient(networkProtocol, "noch nicht benötigt!", "62.138.6.50", 12345, 1024, 2);
            await endpointConnection.connect();
            await endpointConnection.sendMessage("#101", false);
            #endregion

            var isValid = AreCredentialsCorrect(user);
            if (isValid)
            {
                //App.IsUserLoggedIn = true;
                Application.Current.Properties["IsUserLoggedIn"] = true;

                System.Diagnostics.Debug.WriteLine("Ab jetzt gibt es eine Exception wegen des Seitenwechsels");

                //Navigation.InsertPageBefore(new MainPage(), this);
                //await Navigation.PopAsync();

                //await Navigation.PushModalAsync(new MainPage());

                App.LogInSwitch();

                //await Content.Navigation.InsertPageBefore(new MainPage());

                //MainPage = new tbfApp.MainPage();

            }
            else
            {
                messageLabel.Text = "Login failed";
                passwordEntry.Text = string.Empty;
            }
        }

        bool AreCredentialsCorrect(User user)
        {
            //return user.Username == Constants.Username && user.Password == Constants.Password;
            return true;
        }


        void networkProtocol(string message)
        {
            messageLabel.Text = message;
        }
    }
}
