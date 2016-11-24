using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network;
using Xamarin.Forms;

namespace tbfApp
{
    public partial class App : Application
    {
        protected static App app;
        public static SimpleNetworkClient endpointConnection;
        public App()
        {
            InitializeComponent();

            app = this;

            endpointConnection = new SimpleNetworkClient(null, "noch nicht benötigt!", "62.138.6.50", 13001, 8000, 2);
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

                //app.MainPage = new tbfApp.LoginPage();
                app.MainPage = new NavigationPage(new LoginPage()
                {
                    Title = "Login",
                })
                {
                    BarBackgroundColor = Color.FromHex(App.getMenueColor()), //#009acd
                    BarTextColor = Color.White,
                };
                //Navigation.PushModalAsync(new tbfApp.LoginPage());
            }
            else
            {
                app.MainPage = new tbfApp.MainPage();
            }
        }

        public static void NavigateToSignUp()
        {
            app.MainPage.Navigation.PushAsync(new SignUpPage()
            {
                Title = "Sign up"
            });
        }

        public static String getMenueColor()
        {
            if (!Application.Current.Properties.ContainsKey("menueColor"))
            {
                Application.Current.Properties["menueColor"] = "009acd";
                System.Diagnostics.Debug.WriteLine("First menueColor set");
            }
            return Convert.ToString(Application.Current.Properties["menueColor"]);
        }

        public static bool setMenueColor(String newColor)
        {
            Application.Current.Properties["menueColor"] = newColor;
            return true;
        }
    }
}
