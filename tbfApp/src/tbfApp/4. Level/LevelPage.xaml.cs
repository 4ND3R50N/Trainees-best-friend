﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace tbfApp
{
    public partial class LevelPage : ContentPage
    {
        private ScrollView scroll;
        private Label label;
        private StackLayout stack;
        static ActivityIndicator activityIndicator;

        private String workoutID;
        public String _workoutImage;

        public LevelPage(String workoutId, String workoutImage)
        {
            InitializeComponent();
            
            this.workoutID = workoutId;
            this._workoutImage = workoutImage;

            scroll = new ScrollView();

            label = new Label
            {
                Text = "Hier hat etwas nicht funktioniert, versuchen Sie es noch einmal.",
                Font = Font.SystemFontOfSize(NamedSize.Large),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            activityIndicator = new ActivityIndicator()
            {
                Color = Color.Gray,
                IsRunning = true,
                WidthRequest = 80,
                HeightRequest = 80,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            Content = activityIndicator;

            // Build the page.        
            stack = new StackLayout();
            stack.Margin = 10;
            scroll.Content = stack;

            if (workoutID.Equals("XXX"))
            {

                stack.Children.Add(new LevelButton("Level 1", Navigation, this,
                    "Das erste Level ist für Ein- und Wiedereinsteiger geeignet.", "XXX", "stern1.png"));
                activityIndicatorSwitch();
            }
            else
            {
                ServerRequest();
            }
        }

        async void ServerRequest()
        {
            //Login Request
            App.endpointConnection.SetProtocolFunction(this.ServerAnswer);
            await App.Communicate("#207;" + workoutID, this);
        }

        async private void ServerAnswer(string protocol)
        {
            try
            {
                List<string> levelList = new List<string>();
                levelList = protocol.Split(new char[] { ';' }).ToList();

                if (levelList.ElementAt(0).Equals("#208"))       //outerList protocolNumber
                {
                    int levelAmountReceived = levelList.Count - 1;

                    if (levelAmountReceived > 0)
                    {
                        for (int i = 1; i < levelAmountReceived + 1; i++)
                        {
                           List<string> levelDataList = new List<string>();
                            levelDataList = levelList.ElementAt(i).Split(new char[] { '|' }).ToList();
                            //innerList
                            //Element 0 = ID | Element 1 = LevelGrade | Element 2 = Description not implemented now TODO
                            
                            switch (levelDataList.ElementAt(1))
                            {
                                case "1":
                                    stack.Children.Add(new LevelButton("Level 1", Navigation, this, levelDataList.ElementAt(2), levelDataList.ElementAt(0), "stern1.png"));
                                    break;
                                case "2":
                                    stack.Children.Add(new LevelButton("Level 2", Navigation, this, levelDataList.ElementAt(2), levelDataList.ElementAt(0), "sterne2.png"));
                                    break;
                                case "3":
                                    stack.Children.Add(new LevelButton("Level 3", Navigation, this, levelDataList.ElementAt(2), levelDataList.ElementAt(0), "sterne3.png"));
                                    break;
                                case "4":
                                    stack.Children.Add(new LevelButton("Level 4", Navigation, this, levelDataList.ElementAt(2), levelDataList.ElementAt(0), "sterne4.png"));
                                    break;
                                case "5":
                                    stack.Children.Add(new LevelButton("Level 5", Navigation, this, levelDataList.ElementAt(2), levelDataList.ElementAt(0), "sterne5.png"));
                                    break;
                                default:
                                    await DisplayAlert("Fehler", "Kommunikationsproblem, Undefinierte Antwort vom Server! " + levelList.ElementAt(0), "OK");
                                    break;
                            }
                        }
                    }
                    else
                    {
                        await DisplayAlert("Fehler", "Keine Levels für dieses Workout", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Fehler", "Kommunikationsproblem, Undefinierte Antwort vom Server! " + levelList.ElementAt(0), "OK");
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Level konnten nicht geladen werden!", "Bufferlänge in den Einstellungen erhöhen!", "Fortfahren");
                stack.Children.Add(label);
            }

    App.endpointConnection.closeConnection();

            activityIndicatorSwitch();
        }

        private void activityIndicatorSwitch()
        {
            if (activityIndicator.IsRunning)
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
    }
}
