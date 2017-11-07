using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace tbfApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserPage : ContentPage
    {
        public UserPage()
        {
            InitializeComponent();

            Title = "Benutzereinstellungen";

            userIDEntry.Text = "BenutzerID: " + App.GetUserID().ToString();
            usernameEntry.Text = App.GetUsername();
            /*
            forenameEntry.Text = "";
            secondnameEntry.Text = "";
            emailEntry.Text = "";
            */
        }

        async void OnSaveSettingsButtonClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Nicht gespeichert!", "Diese Funktion wird in den nächsten Updates erweitert.", "OK");
            messageLabel.Text = "Nicht gespeichert!";
        }
    }
}