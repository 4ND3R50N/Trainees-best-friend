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
        private ScrollView scroll;
        private StackLayout stack;
        static ActivityIndicator activityIndicator;
        private String roomID;
        public WorkoutPage(String roomID)
        {
            InitializeComponent();

            this.roomID = roomID;

            scroll = new ScrollView();

            activityIndicator = new ActivityIndicator()
            {
                Color = Color.Accent,
                IsRunning = true,
                WidthRequest = 80,
                HeightRequest = 80,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            Content = activityIndicator;

            // Build the page.        
            stack = new StackLayout();
            scroll.Content = stack;

            ServerRequest();

            stack.Children.Add(new WorkoutButton("Every Morning", Navigation, this, "Das Workout für jeden guten Start in den Tag", "workoutID HERE ToDo", "buttonBackground700x100.png"));
        }

        async void ServerRequest()
        {
            //Login Request
            App.endpointConnection.SetProtocolFunction(this.ServerAnswer);
            await App.Communicate("#205;" + roomID, this);
        }

        async private void ServerAnswer(string protocol)
        {
            //await DisplayAlert("Servermessage", protocol, "OK");

            List<string> workoutList = new List<string>();
            workoutList = protocol.Split(new char[] { ';' }).ToList();

            if (workoutList.ElementAt(0).Equals("#206"))
            {
                int workoutAmount;
                int.TryParse(workoutList.ElementAt(1), out workoutAmount);      //outerList Element 1 Amount
                if (workoutAmount > 0)
                {
                    for (int i = 2; i < workoutAmount + 2; i++)
                    {
                        List<string> workoutDataList = new List<string>();
                        workoutDataList = workoutList.ElementAt(i).Split(new char[] { '|' }).ToList();
                        //innerList
                        //Element 0 = ID | Element 1 = Name | Element 2 = Description | Element 3 = IconURL
                        stack.Children.Add(new WorkoutButton(workoutDataList.ElementAt(1), Navigation, this, workoutDataList.ElementAt(2), workoutDataList.ElementAt(0), workoutDataList.ElementAt(3)));
                    }
                }
                else
                {
                    await DisplayAlert("Fehler", "Keine Workouts für diesen Raum", "OK");
                }
            }
            else
            {
                await DisplayAlert("Fehler", "Kommunikationsproblem, Undefinierte Antwort vom Server! "+workoutList.ElementAt(0), "OK");
            }
            App.endpointConnection.closeConnection();

            activityIndicatorSwitch();
        }

        private void activityIndicatorSwitch()
        {
            if (activityIndicator.IsRunning == true)
            {
                activityIndicator.IsRunning = false;

                Content = scroll;
            }
            else
            {
                activityIndicator.IsRunning = true;

                Content = activityIndicator;
            }
        }

        public static async void Load()     //Simulate Load Time
        {
            System.Diagnostics.Debug.WriteLine("Task go to sleep");
            await Task.Delay(3000); //Simulates a 3 second wait
            System.Diagnostics.Debug.WriteLine("Task awake");
            activityIndicator.IsRunning = false;
            System.Diagnostics.Debug.WriteLine("on appearing done");
        }
    }
}
