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
            /*
            Label header = new Label
            {
                Text = "Lable",
                Font = Font.BoldSystemFontOfSize(50),
                HorizontalOptions = LayoutOptions.Center
            };
            */

            var scroll = new ScrollView();
            Content = scroll;

            var webImage = new Image { Aspect = Aspect.AspectFit };
            webImage.Source = new UriImageSource
            {
                Uri = new Uri("http://image.flaticon.com/teams/new/1-freepik.jpg"),
                CachingEnabled = true,
                CacheValidity = new TimeSpan(5, 0, 0, 0),
                
            };

            var im = new Image { Aspect = Aspect.AspectFit };
            //im.Source = "contacts.png";
            im.Source = ImageSource.FromFile("contacts.png");

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => 
            {
                // handle the tap
                OnButtonClicked(s, e);
            };
            //im.GestureRecognizers.Add(tapGestureRecognizer);

            Button button = new Button
            {
                Text = "Click Mee!",
                Font = Font.SystemFontOfSize(NamedSize.Large),
                BorderWidth = 1,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                //Image = webImage,
                Image = "contacts.png",
                WidthRequest = 700,//700
                HeightRequest = 140,//140
            };
            button.Clicked += OnButtonClicked;
            //button.Image = ImageSource.FromUri(new Uri("http://image.flaticon.com/teams/new/1-freepik.jpg"));

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
            //var stack = new StackLayout();
            
            scroll.Content = new StackLayout
            {
                
                Children =
                {
                    //header,
                    button,
                    label,
                    im,
                }
            };
            //stack.Children.Add(new BoxView { BackgroundColor = Color.Red, HeightRequest = 600, WidthRequest = 600, });
            //stack.Children.Add(new Entry());
        }

        void OnButtonClicked(object sender, EventArgs e)
        {
            clickTotal += 1;
            label.Text = String.Format("{0} button click{1}",clickTotal, clickTotal == 1 ? "" : "s");

            Navigation.PushAsync(new WorkoutPage()
            {
                Title = "Workouts"
            });
        }
    }
}
