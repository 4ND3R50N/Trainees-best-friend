using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace tbfApp
{
    public partial class LevelPage : ContentPage
    {
        public LevelPage(String levelID)
        {
            InitializeComponent();

            var scroll = new ScrollView();
            Content = scroll;

            // Build the page.        
            var stack = new StackLayout();
            scroll.Content = stack;

            stack.Children.Add(new LevelButton("Level 1", Navigation, this, "Das erste Level ist für Ein- und Wiedereinsteiger geeignet.", "LevelID HERE ToDo", "stern1.png"));
            stack.Children.Add(new LevelButton("Level 2", Navigation, this, "Level description HERE", "LevelID HERE ToDo", "sterne2.png"));
            stack.Children.Add(new LevelButton("Level 3", Navigation, this, "Level description HERE", "LevelID HERE ToDo", "sterne3.png"));
            stack.Children.Add(new LevelButton("Level 4", Navigation, this, "Level description HERE", "LevelID HERE ToDo", "sterne4.png"));
            stack.Children.Add(new LevelButton("Level 5", Navigation, this, "Level description HERE", "LevelID HERE ToDo", "sterne5.png"));
        }
    }
}
