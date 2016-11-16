using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

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
    }
}
