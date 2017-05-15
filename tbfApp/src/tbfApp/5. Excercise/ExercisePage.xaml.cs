using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace tbfApp
{
    public partial class ExercisePage : CarouselPage
    {
        private ScrollView scroll;
        //private StackLayout stack;
        private String levelID;

        public ExercisePage(String levelID)
        {
            InitializeComponent();

            this.levelID = levelID;

            scroll = new ScrollView();

            if (levelID.Equals("XXX"))
            {
                WebView browser = new WebView();
                HtmlWebViewSource htmlSource = new HtmlWebViewSource();
                htmlSource.Html = @"<html><body><div style=""position:relative;height:0;padding-bottom:56.25%""><iframe src=""https://www.youtube.com/embed/TD-v1b_YVpg?ecver=2"" width=""640"" height=""360"" frameborder=""0"" style=""position:absolute;width:100%;height:100%;left:0"" allowfullscreen></iframe></div></body></html>";
                //htmlSource.Html = @"<html><body><iframe src=""https://www.youtube.com/embed/TD-v1b_YVpg?ecver=2"" width=""640"" height=""360"" frameborder=""0"" style=""position:absolute;width:100%;height:100%;left:0"" allowfullscreen></iframe></body></html>";
                browser.Source = htmlSource;

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
                int workoutAmount;
                int.TryParse(exerciseList.ElementAt(1), out workoutAmount);      //outerList Element 1 Amount
                if (workoutAmount > 0)
                {
                    for (int i = 2; i < workoutAmount + 2; i++)
                    {
                        List<string> exerciseDataList = new List<string>();
                        exerciseDataList = exerciseList.ElementAt(i).Split(new char[] { '|' }).ToList();
                        //innerList
                        //Element 0 = ID | Element 1 = Name | Element 2 = Description | Element 3 = MediaURL
                        WebView browser = new WebView();
                        HtmlWebViewSource htmlSource = new HtmlWebViewSource();
                        //htmlSource.Html = @"<iframe src=""https://drive.google.com/file/d/0Bx6xdBrmrxARaXVpZmJFdDFuVDQ/preview"" width=""640"" height=""360"" frameborder=""0"" style=""position:absolute;width:100%;height:35%;left:0"" allowfullscreen></iframe>";
                        htmlSource.Html = @"<iframe src=" + "\"" + exerciseDataList.ElementAt(3) + "\"" + @" width=""640"" height=""360"" frameborder=""0"" style=""position:absolute;width:100%;height:35%;left:0"" allowfullscreen></iframe>";
                        //htmlSource.Html = @"<html><body><div style=""position:relative;height:0;padding-bottom:56.25%""><iframe src=""https://www.youtube.com/embed/TD-v1b_YVpg?ecver=2"" width=""640"" height=""360"" frameborder=""0"" style=""position:absolute;width:100%;height:100%;left:0"" allowfullscreen></iframe></div></body></html>";
                        browser.Source = htmlSource;

                        Label descriptionLabel = new Label()
                        {
                            Text = exerciseDataList.ElementAt(2)
                        };

                        ScrollView scroll = new ScrollView();

                        Grid grid = new Grid { RowSpacing = 1, ColumnSpacing = 1, };
                        grid.Padding = new Thickness(0, 5, 0, 5);
                        grid.Children.Add(browser);
                        grid.Children.Add(descriptionLabel);

                        scroll.Content = grid;

                        ContentPage contentPage = new ContentPage
                        {
                            Content = grid,
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
        }
    }
}
