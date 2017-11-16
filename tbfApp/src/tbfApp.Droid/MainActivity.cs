using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Octane.Xam.VideoPlayer.Android;

namespace tbfApp.Droid
{
    [Activity(Label = "TBF", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true,
         ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            //Initialize Xamarin Forms Video Player
            //FormsVideoPlayer.Init();

            LoadApplication(new App());

            try
            {
                Window.SetStatusBarColor(Android.Graphics.Color.Argb(255, 0, 0, 0));        //change Status Bar Color to Black
            }
            catch (Exception exception)
            {
                //throw exception;
            }
        }
    }
}

