using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace tbfApp
{
    class WorkoutButton : Grid
    {
        private INavigation Navigation;
        private ContentPage Page;

        private String description;
        private String _workoutId;
        public WorkoutButton(String text, INavigation navigation, ContentPage page, String description, String workoutId, String backgroundImagePath)
        {
            Navigation = navigation;
            Page = page;

            this.description = description;
            this._workoutId = workoutId;

            Label lableButton = new Label
            {
                Text = text,
                Font = Font.BoldSystemFontOfSize(20),
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.End,
                FontFamily = Device.OnPlatform
                (
                    iOS: "DK_Black_Mark_Heavy",
                    Android: "DK_Black_Mark_Heavy.ttf",
                    WinPhone: "Comic Sans MS"
                ),
            };

            var webImage = new Image { Aspect = Aspect.AspectFit };
            webImage.Source = new UriImageSource
            {
                //Uri = new Uri("http://image.flaticon.com/teams/new/1-freepik.jpg"),
                Uri = new Uri("https://upload.wikimedia.org/wikipedia/bar/thumb/e/e7/Logo_TSG_Hoffenheim.svg/510px-Logo_TSG_Hoffenheim.svg.png"),
                CachingEnabled = false,
                CacheValidity = new TimeSpan(5, 0, 0, 0),
            };

            var backgroundImage = new Image
            {
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 700,
                HeightRequest = 100,
            };
            backgroundImage.Source = backgroundImagePath;

            /*
            Button button = new Button
            {
                Font = Font.SystemFontOfSize(NamedSize.Large),
                BorderWidth = 1,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
                //Image = "contacts.png",
                WidthRequest = 700,
                HeightRequest = 100,
                BackgroundColor = Color.FromHex("DADADA"),
            };
            //button.Clicked += OnButtonClicked;
            */

            var info = new Image { Aspect = Aspect.AspectFit };
            info.Source = "info.png";

            var indicator = new ActivityIndicator { Color = new Color(.5), };
            indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsLoading");
            indicator.BindingContext = webImage;

            var grid = new Grid { RowSpacing = 1, ColumnSpacing = 1, };
            var gridButton = new Grid { RowSpacing = 0, ColumnSpacing = 10 };
            var gridLableStars = new Grid { RowSpacing = 0, ColumnSpacing = 0, };
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });

            gridButton.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.02, GridUnitType.Star) });
            //gridButton.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.5, GridUnitType.Star) });
            gridButton.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            gridLableStars.RowDefinitions.Add(new RowDefinition { Height = new GridLength(60) });
            gridLableStars.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30) });

            gridButton.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.1, GridUnitType.Star) });
            gridButton.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.02, GridUnitType.Star) });

            grid.Children.Add(backgroundImage, 0, 0);

            //gridButton.Children.Add(starsIcon, 1, 0);
            gridButton.Children.Add(gridLableStars, 1, 0);
            gridLableStars.Children.Add(lableButton, 0, 0);
            //gridLableStars.Children.Add(backgroundImage, 0, 1);
            gridButton.Children.Add(info, 2, 0);

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
            Navigation.PushAsync(new LevelPage(_workoutId)
            {
                Title = "Level"
            });
        }

        void OnInfoClicked(object sender, EventArgs e)
        {
            Page.DisplayAlert("Workoutinfo", description, "OK");
        }
    }
}
