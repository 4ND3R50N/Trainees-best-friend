using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace tbfApp
{
    public partial class RoomPage : ContentPage
    {
        int clickTotal;
        Label label;
        private ScrollView scroll;
        private StackLayout stack;
        private ActivityIndicator activityIndicator;
        public RoomPage()
        {
            InitializeComponent();

            scroll = new ScrollView();

            //Content.BackgroundColor = Color.Aqua;
            
            label = new Label
            {
                Text = "0 button clicks",
                Font = Font.SystemFontOfSize(NamedSize.Large),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            // Accomodate iPhone status bar.
            this.Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);

            // Build the page.        
            stack = new StackLayout();
            scroll.Content = stack;

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

            ServerRequest();

            stack.Children.Add(new RoomButton("TSG Fitness", Navigation, this, "Der virtuelle Fitnessraum für die Profis der TSG.", "XXX", "https://upload.wikimedia.org/wikipedia/bar/thumb/e/e7/Logo_TSG_Hoffenheim.svg/510px-Logo_TSG_Hoffenheim.svg.png"));
            //stack.Children.Add(new RoomButton("Raum 2", Navigation, this, "Room description here!", "roomID HERE ToDo", "imageURL HERE"));
            //stack.Children.Add(new RoomButton("Raum 3", Navigation, this, "Room description here!", "roomID HERE ToDo", "imageURL HERE"));
            //stack.Children.Add(new RoomButton("Raum 4", Navigation, this, "Room description here!", "roomID HERE ToDo", "imageURL HERE"));
            //stack.Children.Add(new RoomButton("Raum 5", Navigation, this, "Room description here!", "roomID HERE ToDo", "imageURL HERE"));
            //stack.Children.Add(label);
        }

        async void ServerRequest()
        {
            //Login Request
            App.endpointConnection.SetProtocolFunction(this.ServerAnswer);
            await App.Communicate("#211;" + App.GetUserID(), this);
        }

        async private void ServerAnswer(string protocol)
        {
            //await DisplayAlert("Servermessage", protocol, "OK");

            List<string> roomList = new List<string>();
            roomList = protocol.Split(new char[] { ';' }).ToList();

            if (roomList.ElementAt(0).Equals("#212"))       //outerList protocolNumber
            {
                int roomAmount;
                int.TryParse(roomList.ElementAt(1), out roomAmount);        //outerList Element 1 Amount
                if (roomAmount > 0)
                {
                    for (int i = 2; i < roomAmount + 2; i++)
                    {
                        List<string> roomDataList = new List<string>();
                        roomDataList = roomList.ElementAt(i).Split(new char[] { '|' }).ToList();
                        //innerList
                        //Element 0 = ID | Element 1 = Name | Element 2 = Description//Element 0 = ID | Element 1 = Name | Element 2 = Description | Element 3 = isPrivate | Element 4 = IconURL
                        stack.Children.Add(new RoomButton(roomDataList.ElementAt(1), Navigation, this, roomDataList.ElementAt(2), roomDataList.ElementAt(0), roomDataList.ElementAt(4)));
                    }
                }
                else
                {
                    await DisplayAlert("Fehler", "Keine Räume für Sie verfügbar", "OK");
                }
            }
            else
            {
                await DisplayAlert("Fehler", "Kommunikationsproblem, Undefinierte Antwort vom Server! "+ roomList.ElementAt(0), "OK");
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
