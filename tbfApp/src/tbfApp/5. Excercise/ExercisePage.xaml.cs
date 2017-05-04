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
        public ExercisePage(String levelID)
        {
            InitializeComponent();

            var browser = new WebView();
            var htmlSource = new HtmlWebViewSource();
            //htmlSource.Html = @"<html><body><div style=""position:relative;height:0;padding-bottom:56.25%""><iframe src=""https://www.youtube.com/embed/TD-v1b_YVpg?ecver=2"" width=""640"" height=""360"" frameborder=""0"" style=""position:absolute;width:100%;height:100%;left:0"" allowfullscreen></iframe></div></body></html>";
            //htmlSource.Html = @"<html><body><iframe src=""https://www.youtube.com/embed/TD-v1b_YVpg?ecver=2"" width=""640"" height=""360"" frameborder=""0"" style=""position:absolute;width:100%;height:100%;left:0"" allowfullscreen></iframe></body></html>";
            htmlSource.Html = @"<iframe src=""https://drive.google.com/file/d/0Bx6xdBrmrxARaXVpZmJFdDFuVDQ/preview"" width=""640"" height=""360"" frameborder=""0"" style=""position:absolute;width:100%;height:35%;left:0"" allowfullscreen></iframe>";
            browser.Source = htmlSource;

            Label descriptionLabel = new Label()
            {

            };
            descriptionLabel.Text = "Beschreibender Text \nmehrere Zeilen lang";

            ContentPage firstContent = new ContentPage
            {
                Content = browser,
            };

            ContentPage secondContent = new ContentPage
            {
                Content = descriptionLabel,
            };


            Children.Add(firstContent);
            Children.Add(secondContent);
        }
    }
}
