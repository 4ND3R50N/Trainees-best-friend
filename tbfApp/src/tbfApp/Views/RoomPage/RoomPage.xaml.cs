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
        public RoomPage()
        {
            InitializeComponent();
            
            Label headerL = new Label
            {
                Text = "Lii",
                Font = Font.BoldSystemFontOfSize(50),
                HorizontalOptions = LayoutOptions.Center
            };
            Label headerR = new Label
            {
                Text = "Reeeeeee",
                Font = Font.BoldSystemFontOfSize(50),
                HorizontalOptions = LayoutOptions.Center
            };
            Label lableButton = new Label
            {
                Text = "TSG Fitness",
                Font = Font.BoldSystemFontOfSize(25),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };


            var scroll = new ScrollView();
            Content = scroll;

            var webImage = new Image { Aspect = Aspect.AspectFit };
            webImage.Source = new UriImageSource
            {
                //Uri = new Uri("http://image.flaticon.com/teams/new/1-freepik.jpg"),
                Uri = new Uri("https://upload.wikimedia.org/wikipedia/commons/thumb/f/f3/Logo_TSG_Hoffenheim.png/929px-Logo_TSG_Hoffenheim.png"),
                CachingEnabled = false,
                CacheValidity = new TimeSpan(5, 0, 0, 0),
            };

            Button button = new Button
            {
                //Text = "Click Mee!",
                Font = Font.SystemFontOfSize(NamedSize.Large),
                BorderWidth = 1,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                //Image = webImage,
                //Image = "contacts.png",
                WidthRequest = 700,//700
                HeightRequest = 140,//140
                BackgroundColor = Color.Gray,
            };
            button.Clicked += OnButtonClicked;

            var im = new Image { Aspect = Aspect.AspectFit };
            //im.Source = "contacts.png";
            im.Source = ImageSource.FromFile("contacts.png");

            var info = new Image { Aspect = Aspect.AspectFit };
            info.Source = "info.png";
            //info.Source = ImageSource.FromFile("info.png");

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (object sender, EventArgs e) => 
            {
                // handle the tap
                OnInfoClicked(sender, e);
            };
            info.GestureRecognizers.Add(tapGestureRecognizer);

            //Content.BackgroundColor = Color.Aqua;
            
            var indicator = new ActivityIndicator { Color = new Color(.5), };
            indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsLoading");
            indicator.BindingContext = webImage;
            
            BoxView boxView = new BoxView
            {
                Color = Color.Aqua,
                WidthRequest = 700,
                HeightRequest = 140,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                
            };

            var grid = new Grid { RowSpacing = 1, ColumnSpacing = 1, };
            var gridButton = new Grid { RowSpacing = 0, ColumnSpacing = 10 };
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(140) });
            gridButton.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.02, GridUnitType.Star) });
            gridButton.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.5, GridUnitType.Star) });
            gridButton.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            gridButton.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.15, GridUnitType.Star) });
            gridButton.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.02, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(140) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(140) });

            grid.Children.Add(button, 0, 0);
            gridButton.Children.Add(indicator, 1, 0);
            gridButton.Children.Add(webImage,1,0);
            gridButton.Children.Add(lableButton, 2, 0);
            gridButton.Children.Add(info,3,0);

            grid.Children.Add(gridButton, 0, 0);
            //grid.Children.Add(indicator,0,1);

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
            var stack = new StackLayout();
            scroll.Content = stack;
            //stack.Children.Add(grid);
            stack.Children.Add(new RoomButton("TSG Fitness", Navigation, this, "Der virtuelle Fitnessraum für die Profis der TSG", "roomID HERE ToDo"));
            stack.Children.Add(new RoomButton("Raum 2", Navigation, this, "Room description here!", "roomID HERE ToDo"));
            stack.Children.Add(new RoomButton("Raum 3", Navigation, this, "Room description here!", "roomID HERE ToDo"));
            stack.Children.Add(new RoomButton("Raum 4", Navigation, this, "Room description here!", "roomID HERE ToDo"));
            stack.Children.Add(new RoomButton("Raum 5", Navigation, this, "Room description here!", "roomID HERE ToDo"));
            stack.Children.Add(label);
        }

        void OnButtonClicked(object sender, EventArgs e)
        {
            clickTotal += 1;
            label.Text = String.Format("{0} button click{1}",clickTotal, clickTotal == 1 ? "" : "s");

            Navigation.PushAsync(new WorkoutPage("roomID HERE ToDo")
            {
                Title = "Workouts"
            });
        }

        void OnInfoClicked(object sender, EventArgs e)
        {
            DisplayAlert("Roominfo", "Room description here!", "OK");
        }
    }
}
