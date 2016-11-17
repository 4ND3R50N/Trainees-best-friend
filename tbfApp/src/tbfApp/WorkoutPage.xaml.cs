using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using Xamarin.Forms;

namespace tbfApp
{
    public partial class WorkoutPage : ContentPage
    {
        static ActivityIndicator activityIndicator;
        public WorkoutPage()
        {
            InitializeComponent();

            activityIndicator = new ActivityIndicator
            {
                IsRunning = true,
                Color = Color.Accent,
                WidthRequest = 50,
                HeightRequest = 50,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            // Build the page.
            this.Content = new StackLayout
            {
                Children =
                {
                    activityIndicator
                },
                BackgroundColor = Color.Black
            };

            Load();
        }

        public static async void Load()
        {
            System.Diagnostics.Debug.WriteLine("Task go to sleep");
            await Task.Delay(3000); //Simulates a 3 second wait
            System.Diagnostics.Debug.WriteLine("Task awake");
            activityIndicator.IsRunning = false;
            System.Diagnostics.Debug.WriteLine("on appearing done");
        }
    }
}
