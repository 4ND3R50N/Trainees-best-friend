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

        public MasterPage()
        {
            InitializeComponent();
            Content.BackgroundColor = Color.FromHex("009acd");

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
                TargetType = typeof(LevelPage)
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
