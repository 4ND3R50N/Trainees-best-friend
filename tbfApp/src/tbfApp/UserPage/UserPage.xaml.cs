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

            // Positioning of the ActivityIndicator in Center
            activityIndicator.WidthRequest = 80;
            activityIndicator.HeightRequest = 80;
            Constraint centerX = Constraint.RelativeToParent(parent => (parent.Width / 2) - (activityIndicator.Width / 2));
            Constraint centerY = Constraint.RelativeToParent(parent => (parent.Height / 2) - (activityIndicator.Height / 2));
            relativeLayout.Children.Add(activityIndicator, centerX, centerY);

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