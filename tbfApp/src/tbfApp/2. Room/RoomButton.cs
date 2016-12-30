using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace tbfApp
{
    class RoomButton : Grid
    {
        private INavigation Navigation;
        private ContentPage Page;

        private String description;
        private String _roomID;
        public RoomButton(String text, INavigation navigation, ContentPage page, String description, String roomID, String iconURL)
        {
            Navigation = navigation;
            Page = page;

            this.description = description;
            this._roomID = roomID;

            Label lableButton = new Label
            {
                Text = text,
                Font = Font.BoldSystemFontOfSize(25),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };

            var webImage = new Image { Aspect = Aspect.AspectFit };
            webImage.Source = new UriImageSource
            {
                //Uri = new Uri("http://image.flaticon.com/teams/new/1-freepik.jpg"),
                Uri = new Uri("https://upload.wikimedia.org/wikipedia/bar/thumb/e/e7/Logo_TSG_Hoffenheim.svg/510px-Logo_TSG_Hoffenheim.svg.png"),
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
                BackgroundColor = Color.FromHex("EEEBEA"),
            };
            //button.Clicked += OnButtonClicked;

            var info = new Image { Aspect = Aspect.AspectFit };
            info.Source = "info.png";

            var indicator = new ActivityIndicator { Color = new Color(.5), };
            indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsLoading");
            indicator.BindingContext = webImage;

            var grid = new Grid { RowSpacing = 1, ColumnSpacing = 1, };
            var gridButton = new Grid { RowSpacing = 0, ColumnSpacing = 10 };
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(140) });
            gridButton.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.02, GridUnitType.Star) });
            gridButton.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.5, GridUnitType.Star) });
            gridButton.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            gridButton.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.15, GridUnitType.Star) });
            gridButton.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.02, GridUnitType.Star) });

            grid.Children.Add(button, 0, 0);

            gridButton.Children.Add(indicator, 1, 0);
            gridButton.Children.Add(webImage, 1, 0);
            gridButton.Children.Add(lableButton, 2, 0);
            gridButton.Children.Add(info, 3, 0);

            grid.Children.Add(gridButton, 0, 0);

            var tapGestureRecognizerButton = new TapGestureRecognizer();
            tapGestureRecognizerButton.Tapped += (object sender, EventArgs e) =>
            {
                // handle the tap
                OnButtonClicked(sender, e);
            };
            gridButton.GestureRecognizers.Add(tapGestureRecognizerButton);

            var tapGestureRecognizerInfo = new TapGestureRecognizer();
            tapGestureRecognizerInfo.Tapped += (object sender, EventArgs e) =>
            {
                // handle the tap
                OnInfoClicked(sender, e);
            };
            info.GestureRecognizers.Add(tapGestureRecognizerInfo);

            this.Children.Add(grid);
        }

        void OnButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new WorkoutPage(_roomID)
            {
                Title = "Workouts"
            });
        }

        void OnInfoClicked(object sender, EventArgs e)
        {
            Page.DisplayAlert("Roominfo", description, "OK");
        }
    }
}
