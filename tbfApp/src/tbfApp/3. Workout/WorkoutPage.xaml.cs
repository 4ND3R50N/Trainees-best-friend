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
        public WorkoutPage(String roomID)
        {
            InitializeComponent();

            var scroll = new ScrollView();
            Content = scroll;

            activityIndicator = new ActivityIndicator
            {
                IsRunning = true,
                Color = Color.Accent,
                WidthRequest = 50,
                HeightRequest = 50,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };

            /*
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
            */

            // Build the page.        
            var stack = new StackLayout();
            scroll.Content = stack;

            stack.Children.Add(new WorkoutButton("Every Morning", Navigation, this, "Das Workout für jeden guten Start in den Tag", "workoutID HERE ToDo", "buttonBackground700x100.png"));

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
