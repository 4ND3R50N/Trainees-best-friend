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
        bool isRunning = true;
        ActivityIndicator activityIndicator;
        public WorkoutPage()
        {
            InitializeComponent();

            activityIndicator = new ActivityIndicator
            {
                IsRunning = isRunning,
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
                }
            };

            Task.Delay(3000);
            isRunning = false;
            //OnAppearing();
            Update("TextToChange");
        }

        public void Update(string text)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.Content = new StackLayout
                {
                    Children =
                    {
                        //new Label { Text = text }
                        activityIndicator
                    }
                };
            });
        }
    }
}
