using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace tbfApp
{
    public partial class MasterPage : ContentPage
    {
        public ListView ListView { get { return listView; } }
        public String listColor;

        public MasterPage()
        {
            InitializeComponent();

            listView.BackgroundColor = Color.FromHex(App.GetMenueColor());

            var masterPageItems = new List<MasterPageItem>();
            
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Räume",
                IconSource = "rooms.png",
                TargetType = typeof(RoomPage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Einstellungen",
                IconSource = "todo.png",
                TargetType = typeof(SettingsPage)
            });
            /*
            masterPageItems.Add(new MasterPageItem
            {
                Title = "lo",
                //IconSource = "contacts.png",
                TargetType = typeof(RoomPage),
            });
            */
            masterPageItems.Add(new MasterPageItem
            {
                Title = App.GetUsername(),
                IconSource = "contacts.png",
                TargetType = typeof(RoomPage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Abmelden",
                IconSource = "logoff.png",
                TargetType = typeof(LogOut)
            });
            
            listView.ItemsSource = masterPageItems;
        }
    }
}
