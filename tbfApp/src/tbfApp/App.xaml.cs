using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace tbfApp
{
    public partial class App : Application
    {
        //public static bool IsUserLoggedIn { get; set; }
        protected static App app;
        public App()
        {
            InitializeComponent();

            app = this;
        }

        protected override void OnStart()
        {
            System.Diagnostics.Debug.WriteLine("App OnStart");
            if(!Application.Current.Properties.ContainsKey("IsUserLoggedIn"))
            {
                Application.Current.Properties["IsUserLoggedIn"] = false;
                System.Diagnostics.Debug.WriteLine("First Appstart, initialize Current.Properties");
            }

            LogInSwitch();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            System.Diagnostics.Debug.WriteLine("App OnSleep");
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            System.Diagnostics.Debug.WriteLine("App resumed");
        }

        public static void LogInSwitch()
        {
            System.Diagnostics.Debug.WriteLine("LogInSwitch");

            //App.IsUserLoggedIn = true;
            bool IsUserLoggedIn = Convert.ToBoolean(Application.Current.Properties["IsUserLoggedIn"]);
            if (!IsUserLoggedIn)
            {
                //app.MainPage = new NavigationPage(new tbfApp.LoginPage());

                //Navigation.PushModalAsync(new tbfApp.LoginPage());

                app.MainPage = new tbfApp.LoginPage();
                //Navigation.PushModalAsync(new tbfApp.LoginPage());
            }
            else
            {
                app.MainPage = new tbfApp.MainPage();
            }
        }
    }
}
