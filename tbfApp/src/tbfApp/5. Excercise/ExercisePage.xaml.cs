using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tbfApp.Network;
using Xamarin.Forms;

namespace tbfApp
{
    public partial class ExercisePage : CarouselPage
    {
        private LevelPage parrentLevelPage;
        private ContentPage contentPage;

        private ScrollView scroll;
        //private StackLayout stack;
        private String levelID;
        static ActivityIndicator activityIndicator;

        public ExercisePage(String levelID, LevelPage parrentLevelPage)
        {
            InitializeComponent();

            this.parrentLevelPage = parrentLevelPage;

            this.levelID = levelID;

            scroll = new ScrollView();

            activityIndicator = new ActivityIndicator()
            {
                Color = Color.Gray,
                IsRunning = false,
                WidthRequest = 80,
                HeightRequest = 80,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            activityIndicatorSwitch();

            if (levelID.Equals("XXX"))
            {
                WebView browser = new WebView();
                HtmlWebViewSource htmlSource = new HtmlWebViewSource();
                htmlSource.Html = @"<html><body><div style=""position:relative;height:0;padding-bottom:56.25%""><iframe src=""https://www.youtube.com/embed/TD-v1b_YVpg?ecver=2"" width=""640"" height=""360"" frameborder=""0"" style=""position:absolute;width:100%;height:100%;left:0"" allowfullscreen></iframe></div></body></html>";
                //htmlSource.Html = @"<html><body><iframe src=""https://www.youtube.com/embed/TD-v1b_YVpg?ecver=2"" width=""640"" height=""360"" frameborder=""0"" style=""position:absolute;width:100%;height:100%;left:0"" allowfullscreen></iframe></body></html>";
                browser.Source = htmlSource;

                /* //Video Player Test
                VideoPlayer videoPlayer = new VideoPlayer();
                videoPlayer.Source = "http://62.138.6.50:13010/Seitliche_%C3%9Cbung_Stufe_3.mp4";
                videoPlayer.FillMode = FillMode.ResizeAspectFill;
                videoPlayer.DisplayControls = true;
                */

                ContentPage firstContent = new ContentPage
                {
                    Content = browser,
                };
                Children.Add(firstContent);

                ContentPage secondContent = new ContentPage
                {
                    //Content = descriptionLabel,
                };
                Children.Add(secondContent);
                Label descriptionLabel = new Label()
                {

                };
                descriptionLabel.Text = "Beschreibender Text \nmehrere Zeilen lang";
                secondContent.Content = descriptionLabel;

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
            await App.Communicate("#209;" + levelID, this);
        }

        async private void ServerAnswer(string protocol)
        {
            //await DisplayAlert("Servermessage", protocol, "OK");

            List<string> exerciseList = new List<string>();
            exerciseList = protocol.Split(new char[] { ';' }).ToList();

            if (exerciseList.ElementAt(0).Equals("#210"))       //outerList protocolNumber
            {
                int exerciseAmountReceived = exerciseList.Count - 2;

                int exerciseAmountServer;
                int.TryParse(exerciseList.ElementAt(1), out exerciseAmountServer);      //outerList Element 1 Amount

                if (exerciseAmountServer != exerciseAmountReceived)
                {
                    await DisplayAlert("Nicht alle Exercise wurden geladen!", "Pufferlänge in den Einstellungen erhöhen.",
                        "Fortfahren");
                }

                if (exerciseAmountReceived > 0)
                {
                    for (int i = 2; i < exerciseAmountReceived + 2; i++)
                    {
                        List<string> exerciseDataList = new List<string>();
                        exerciseDataList = exerciseList.ElementAt(i).Split(new char[] { '|' }).ToList();
                        //innerList
                        //Element 0 = ID | Element 1 = Name | Element 2 = Description | Element 3 = MediaURL
                        MyWebView browser = new MyWebView();                       //Use own WebView with own custom renderer
                        HtmlWebViewSource htmlSource = new HtmlWebViewSource();

                        string testVideo = "http://tbf.spdns.de/betatest_videos/uebung_03.mp4";

                        //1.   htmlSource.Html = @"<iframe src=" + "\"" + exerciseDataList.ElementAt(3) + "\"" + @" width=""640"" height=""360"" frameborder=""0"" style=""position:absolute;width:100%;height:35%;left:0"" allowfullscreen></iframe>";
                        
                        htmlSource.Html = $"<video controls width=100% height=300 poster=\"{parrentLevelPage._workoutImage}\"> <source src=\"{exerciseDataList.ElementAt(3)}\" type=\"video/mp4\"> </video>";

                        /* 2.
                        htmlSource.Html = @"<!DOCTYPE html>
                                            <html>
                                              <head>
                                                <title>Video mit Vorschau anzeigen</title>
                                              </head>
                                              <body>
                                                <video controls autoplay height=""200"" width=""300"" poster=""SeitlicheUebung.jpg""> 
                                                <source src=" + "\"" + exerciseDataList.ElementAt(3) + "\"" + @" type=""video/mp4"">
                                                </video>
                                              </body>
                                            </html>";
                        */

                        /* 3.
                         htmlSource.Html = @"<video controls autoplay width=""340"" height=""160"" poster=" + "\"" + parrentLevelPage._workoutImage + "\"" + @"> 
                                            <source src=" + "\"" + exerciseDataList.ElementAt(3) + "\"" + @" type=""video/mp4"">
                                            </video>";
                         */

                        browser.Source = htmlSource;
                        //browser.Source = exerciseDataList.ElementAt(3);

                        Label descriptionLabel = new Label()
                        {
                            Text = exerciseDataList.ElementAt(2),
                        };

                        ScrollView scroll = new ScrollView();

                        Grid grid = new Grid { RowSpacing = 1, ColumnSpacing = 1, };
                        //grid.Padding = new Thickness(0, 5, 0, 5);
                        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(300) });            //Videoelemt Height
                        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });            //Space Height between video and description
                        grid.RowDefinitions.Add(new RowDefinition {  });            //Description Height
                        //grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                        //grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1 , GridUnitType.Star) });

                        grid.Children.Add(browser, 0, 0);
                        grid.Children.Add(descriptionLabel, 0, 2);

                        scroll.Content = grid;

                        contentPage = new ContentPage
                        {
                            Content = scroll,
                        };
                        Children.Add(contentPage);
                    }
                }
                else
                {
                    await DisplayAlert("Fehler", "Keine Exercises für dieses Level", "OK");
                }
            }
            else
            {
                await DisplayAlert("Fehler", "Kommunikationsproblem, Undefinierte Antwort vom Server! " + exerciseList.ElementAt(0), "OK");
            }
            App.endpointConnection.closeConnection();

            activityIndicatorSwitch();
        }
        private void activityIndicatorSwitch()
        {
            if (activityIndicator.IsRunning)
            {
                activityIndicator.IsRunning = false;

                //Content = scroll;
                Children.RemoveAt(0);
            }
            else
            {
                activityIndicator.IsRunning = true;

                //Content = activityIndicator;
                ContentPage activityIndicatorPage = new ContentPage
                {
                    Content = activityIndicator,
                };
                Children.Insert(0, activityIndicatorPage);
            }
        }
    }
}
