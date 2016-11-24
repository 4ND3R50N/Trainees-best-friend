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

            listView.BackgroundColor = Color.FromHex(App.getMenueColor());

            var masterPageItems = new List<MasterPageItem>();
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Räume",
                IconSource = "contacts.png",
                TargetType = typeof(RoomPage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Einstellungen",
                IconSource = "todo.png",
                TargetType = typeof(SettingsPage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Abmelden",
                IconSource = "reminders.png",
                TargetType = typeof(LogOut)
            });
            
            listView.ItemsSource = masterPageItems;
        }
    }
}
