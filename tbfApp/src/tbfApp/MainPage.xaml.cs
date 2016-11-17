using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace tbfApp
{
    public partial class MainPage : MasterDetailPage
    {
        //MasterPage masterPage;
        public MainPage()
        {
            masterPage = new MasterPage()
            {
                //Title = "Menü",
                BackgroundColor = Color.FromHex("#009acd"),
            };
            Master = masterPage;
            Detail = new NavigationPage(new RoomPage()
            {
                Title = "Räume",
            })
            {
                BarBackgroundColor = Color.FromHex("#009acd"),
                BarTextColor = Color.White,
            };

            masterPage.ListView.ItemSelected += OnItemSelected;
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType))
                {
                    BarBackgroundColor = Color.FromHex("#009acd"),
                    BarTextColor = Color.White,
                };
                masterPage.ListView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}
