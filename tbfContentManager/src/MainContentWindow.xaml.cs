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
    /// Interaktionslogik für MainContentWindow.xaml
    /// </summary>
    public partial class MainContentWindow
    {
        public MainContentWindow()
        {
            InitializeComponent();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            MainWindow newLogin = new MainWindow();
            newLogin.Show();
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btn_add_room_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
