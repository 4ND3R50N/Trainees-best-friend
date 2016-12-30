using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace tbfApp
{
    class LogOut:ContentPage
    {
        public LogOut()
        {
            Application.Current.Properties["IsUserLoggedIn"] = false;
            App.LogInSwitch();
        }
    }
}
