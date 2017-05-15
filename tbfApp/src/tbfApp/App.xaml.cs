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

            endpointConnection = new SimpleNetworkClient(null, "noch nicht benötigt!", GetServerAdress(), GetServerPort(), GetServerBufferlenght(), 0);
        }

        protected override void OnStart()
        {
            System.Diagnostics.Debug.WriteLine("App OnStart");
            if (!Application.Current.Properties.ContainsKey("IsUserLoggedIn"))
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
                    BarBackgroundColor = Color.FromHex(App.GetMenueColor()), //#009acd
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

        public static async Task<bool> Communicate(String protocolMessage, Page page)
        {
            try
            {
                await App.endpointConnection.connect();
                await App.endpointConnection.sendMessage(protocolMessage, false);
                return true;
            }
            catch (Exception)
            {
                try
                {
                    //ping Google TODO

                    //if (false) //Reachability.IsHostReachable("http://google.com")

                    /*
                    var request = HttpWebRequest.Create("http://google.com");
                    request.ContentType = "application/json";
                    request.Method = "GET";
                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                        {

                        }
                    }
                    */

                    /*
                    SimpleNetworkClient testConnection = new SimpleNetworkClient(null, "noch nicht benötigt!", "http://google.com", 80, 4000, 2);
                    testConnection.SetProtocolFunction(ServerAnswer);
                    await testConnection.connect();
                    //await testConnection.sendMessage("GET",false);
                    */

                    var answer =
                        await page.DisplayAlert("Verbindungsproblem", "Server nicht erreichbar!", "OK", "Wiederholen");
                    if (!answer)
                    {
                        System.Diagnostics.Debug.WriteLine("Reload");
                        //reload Site TODO
                        await App.Communicate(protocolMessage, page);
                    }
                }
                catch (Exception)
                {
                    var answer =
                        await page.DisplayAlert("Verbindungsproblem", "Keine Internetverbindung!", "OK", "Wiederholen");
                    if (!answer)
                    {
                        System.Diagnostics.Debug.WriteLine("Reload");
                        //reload Site TODO
                        await App.Communicate(protocolMessage, page);
                    }
                }
            }
            return false;
        }

        public static String GetMenueColor()
        {
            if (!Application.Current.Properties.ContainsKey("menueColor"))
            {
                Application.Current.Properties["menueColor"] = "009acd";
                System.Diagnostics.Debug.WriteLine("First menueColor set");
            }
            return Convert.ToString(Application.Current.Properties["menueColor"]);
        }

        public static bool SetMenueColor(String newColor)
        {
            Application.Current.Properties["menueColor"] = newColor;
            return true;
        }

        public static String GetServerAdress()
        {
            if (!Application.Current.Properties.ContainsKey("serverAdress"))
            {
                Application.Current.Properties["serverAdress"] = "62.138.6.50";
                System.Diagnostics.Debug.WriteLine("First serverAdress set");
            }
            return Convert.ToString(Application.Current.Properties["serverAdress"]);
        }

        public static bool SetServerAdress(String newServerAdress)
        {
            Application.Current.Properties["serverAdress"] = newServerAdress;
            return true;
        }

        public static short GetServerPort()
        {
            if (!Application.Current.Properties.ContainsKey("serverPort"))
            {
                Application.Current.Properties["serverPort"] = 13001;
                System.Diagnostics.Debug.WriteLine("First serverPort set");
            }
            return Convert.ToInt16(Application.Current.Properties["serverPort"]);
        }

        public static bool SetServerPort(int newServerPort)
        {
            Application.Current.Properties["serverPort"] = newServerPort;
            return true;
        }

        public static short GetServerBufferlenght()
        {
            if (!Application.Current.Properties.ContainsKey("serverBufferlenght"))
            {
                Application.Current.Properties["serverBufferlenght"] = 8000;
                System.Diagnostics.Debug.WriteLine("First serverBufferlenght set");
            }
            return Convert.ToInt16(Application.Current.Properties["serverBufferlenght"]);
        }

        public static bool SetServerBufferlenght(int newServerBufferlenght)
        {
            Application.Current.Properties["serverBufferlenght"] = newServerBufferlenght;
            return true;
        }

        public static int GetUserID()
        {
            if (!Application.Current.Properties.ContainsKey("userID"))
            {
                Application.Current.Properties["userID"] = 0;
                System.Diagnostics.Debug.WriteLine("First userID set");
            }
            return Convert.ToInt32(Application.Current.Properties["userID"]);
        }

        public static bool SetUserID(int newUserID)
        {
            try
            {
                Application.Current.Properties["userID"] = newUserID;
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("USER ID IS NOT SET!!!!!!!!");
                //throw;
            }
            return true;
        }

        public static String GetUsername()
        {
            if (!Application.Current.Properties.ContainsKey("username"))
            {
                Application.Current.Properties["username"] = "Benutzername";
                System.Diagnostics.Debug.WriteLine("First username set");
            }
            return Convert.ToString(Application.Current.Properties["username"]);
        }
        public static bool SetUsername(String newUsername)
        {
            Application.Current.Properties["username"] = newUsername;
            return true;
        }
    }
}
