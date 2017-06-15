using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Android.App;

using Android.Content;
using Android.Content.PM;
using Android.Webkit;
using Android.Widget;
using tbfApp;
using tbfApp.Network;
using Xamarin.Forms;
using View = Android.Views.View;
using WebView = Xamarin.Forms.WebView;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("tbfApp.Droid")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("tbfApp.Droid")]
[assembly: AssemblyCopyright("Copyright ©  2017")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

// Add some common permissions, these can be removed if not needed
[assembly: UsesPermission(Android.Manifest.Permission.Internet)]
[assembly: UsesPermission(Android.Manifest.Permission.WriteExternalStorage)]

#if DEBUG   //Deploy optimation from Xamarin Forum
[assembly: Application(Debuggable = true)]
#else
[assembly: Application(Debuggable=false)]
#endif

//Own WebView for Android
[assembly: ExportRenderer(typeof(MyWebView), typeof(MyWebViewRenderer))]
namespace tbfApp
{

    using Xamarin.Forms.Platform.Android;
    
    public class MyWebViewRenderer : WebViewRenderer
    {
        WebView w;
        Android.Widget.RelativeLayout container;
        float dp;
        static FrameLayout fullscreenV;
        
        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);
            //setContentView
            //dp = getDp(this);

            //container = (Android.Widget.RelativeLayout)FindViewById(R);

            //w = (WebView)findViewById(R.id.w);

            if (fullscreenV != null && fullscreenV.Parent == null)
            {
                w.IsVisible = true;
                container.AddView(fullscreenV);
            }

            var webView = new global::Android.Webkit.WebView(this.Context);
            //webView.SetWebChromeClient(new CrmClient(this.Context));
            webView.SetWebChromeClient(new WebChromeClient());
            webView.SetWebViewClient(new WebViewClient());
            webView.Settings.JavaScriptEnabled = true;
            //webView.Settings.SetPluginState(WebSettings.PluginState.On);
            //this.SetNativeControl(webView);
            webView.Settings.MediaPlaybackRequiresUserGesture = false;

        }
        /*
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView();

            //dp = GetDatabasePath(this);
            container = (Android.Widget.RelativeLayout) FindViewById(this.TaskId);
        }
        */
        class CrmClient : WebChromeClient
        {
            Activity a;

            public CrmClient(Activity a)
            {
                this.a = a;
            }

            public override void OnShowCustomView(View view, ICustomViewCallback callback)
            {
                base.OnShowCustomView(view, callback);
                fullscreenV = (FrameLayout)view;
                a.RequestedOrientation = ScreenOrientation.Landscape;
            }

            public override void OnHideCustomView()
            {
                base.OnHideCustomView();
                fullscreenV = null;
                a.StartActivity(new Intent(a, typeof(MyWebViewRenderer)).SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask));

            }
        }
    }

}