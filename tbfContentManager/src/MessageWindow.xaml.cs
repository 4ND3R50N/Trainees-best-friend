using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace tbfContentManager
{
    /// <summary>
    /// Interaktionslogik für MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow
    {
        bool pb1Enabled;
        public bool preventClose;

        public MessageWindow(Window owner, bool preventClose = false)
        {
            pb1Enabled = false;
            this.preventClose = preventClose;
            Owner = owner;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            InitializeComponent();

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(preventClose)
                e.Cancel = true;
        }

        public string UpperText
        {
            get
            {
                return (string) labelUpper.Content;
            }
            set
            {
                if(labelUpper.Content == null)
                {
                    Height += 75;
                    labelUpper.Visibility = Visibility.Visible;
                }
                labelUpper.Content = value;
            }
        }
        public string LowerText
        {
            get
            {
                return (string)labelLower.Content;
            }
            set
            {
                if (labelLower.Content == null)
                {
                    Height += 75;
                    labelLower.Visibility = Visibility.Visible;
                }
                labelLower.Content = value;
            }
        }
        /*
        public double Progress
        {
            get
            {
                return pb1.Value;
            }
            set
            {
                if (!pb1Enabled)
                {
                    pb1Enabled = true;
                    Height += 25;
                    pb1.Visibility = Visibility.Visible;
                }
                pb1.Value = value;
            }
        }
        */
    }
}
